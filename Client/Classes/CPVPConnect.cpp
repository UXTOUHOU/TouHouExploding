#include "CPVPConnect.h"
#include "CPVPMode.h"
#include "json\rapidjson.h"
#include "json\stringbuffer.h"
#include "json\writer.h"
#include <iostream>

using namespace rapidjson;

CPVPConnect *CPVPConnect::pPVPConnect = NULL;
map<OrderType, vector<OrderParam> > g_mapOrderParam;

void addOrderParam(OrderType order, ...)
{
	va_list orderParamList;
	va_start(orderParamList, order);

	vector<OrderParam> *vecParam = &g_mapOrderParam[order];
	OrderParam *param;
	while ((param = va_arg(orderParamList, OrderParam *)) != NULL)
	{
		vecParam->push_back(*param);
	}
}

CPVPConnect::CPVPConnect()
{
	init();
}

CPVPConnect::~CPVPConnect()
{
}

void CPVPConnect::init()
{
	_nowOrderID = 0;

	addOrderParam(OT_CS_SendChat, 
		&OrderParam("Message", kStringType),
		NULL);
	addOrderParam(OT_CS_Login,
		&OrderParam("PlayerName", kStringType),
		&OrderParam("PassWord", kStringType),
		NULL);
	addOrderParam(OT_CS_AskRoomList,
		NULL);
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//addOrderParam();
	//
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(1, 1), &wsa) != 0)
	{
		CCASSERT(false, "socket加载失败");
		return;
	}
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_port = 2333;
	serverAddr.sin_addr.s_addr = htonl(INADDR_LOOPBACK);

	clientSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	int ret = connect(clientSocket, (sockaddr *)&serverAddr, sizeof(serverAddr));
	log("connect ret", ret);
	char t[10];
	setsockopt(clientSocket, IPPROTO_TCP, TCP_NODELAY, t, sizeof(t));
}

void CPVPConnect::_sendToServer(std::string str)
{
	::send(clientSocket, str.c_str(), str.length(), 0);
}

//void CPVPConnect::keepConnect()
//{
//	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_KeepConnect),
//		NULL));
//}
//
//void CPVPConnect::sendChat(string msg)
//{
//	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_SendChat),
//		&JsonElement("Message", kStringType, &msg),
//		NULL));
//}
//
void CPVPConnect::login(const string &playerName, const string &passWord)
{
	_sendToServer(_makeJsonString(OT_CS_Login, &playerName, &passWord));
}
//
//void CPVPConnect::versionCheck(int version)
//{
//	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_VersionCheck),
//		&JsonElement("ClientVersion", kNumberType, version),
//		NULL));
//}
//
//void CPVPConnect::unitMove(int x, int y)
//{
//	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_UnitMove),
//		&JsonElement("PositionX", kNumberType, x),
//		&JsonElement("PositionY", kNumberType, y),
//		NULL));
//}
//
//void CPVPConnect::unitAttack(int x, int y)
//{
//	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_UnitAttack),
//		&JsonElement("PositionX", kNumberType, x),
//		&JsonElement("PositionY", kNumberType, y),
//		NULL));
//}
//
//void CPVPConnect::useCard(int cardID)
//{
//	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_UseCard),
//		&JsonElement("CardID", kNumberType, cardID),
//		NULL));
//}
//
//void CPVPConnect::useSkill(int skillID)
//{
//	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_UseSkill),
//		&JsonElement("CardID", kNumberType, skillID),
//		NULL));
//}
//
//void CPVPConnect::summon(int unitID)
//{
//	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_Summon),
//		&JsonElement("CardID", kNumberType, unitID),
//		NULL));
//}
//
//void CPVPConnect::endTurn()
//{
//	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_EndTurn),
//		NULL));
//}
//
void CPVPConnect::askRoomList()
{
	_sendToServer(_makeJsonString(OT_CS_AskRoomList, NULL));
}

//void CPVPConnect::parseAnnounceChat(const string &strJSON, string &playerName, string &msg)
//{
//	_parse(strJSON, OT_SC_AnnounceChat, &playerName, &msg, NULL);
//}

OrderType CPVPConnect::getOrderType(const string &strJSON)
{
	Document document;
	document.Parse<0>(strJSON.c_str());
	if (document.HasParseError())
	{
		CCAssert(false, "JSON结构错误");
		return OrderType::OT_NULL;
	}

	return (OrderType)document["OrderType"].GetInt();
}

void CPVPConnect::parse(const string &strJSON, OrderType order, ...)
{
	Document document;
	document.Parse<0>(strJSON.c_str());
	if (document.HasParseError())
	{
		CCAssert(false, "JSON结构错误");
		return;
	}

	auto it = g_mapOrderParam.find(order);
	if (it == g_mapOrderParam.end())
	{
		CCAssert(false, "未设置命令参数格式");
		return;
	}
	auto &vecParam = it->second;

	va_list vaOrderParam;
	va_start(vaOrderParam, order);
	for (auto itParam : vecParam)
	{
		switch (itParam.type)
		{
		case kStringType:
		{
			string *res = va_arg(vaOrderParam, string *);
			*res = document[itParam.name].GetString();
		}
			break;
		case kNumberType:
		{
			int *res = va_arg(vaOrderParam, int *);
			*res = document[itParam.name].GetInt();
		}
			break;
		default:
			break;
		}
	}
}

string CPVPConnect::_makeJsonString(OrderType order, ...)
{
	va_list listObject;
	va_start(listObject, order);

	Document document;
	Document::AllocatorType& allocator = document.GetAllocator();
	rapidjson::Value root(kObjectType);

	rapidjson::Value vOrderID(kNumberType);
	vOrderID.SetInt(++_nowOrderID);
	root.AddMember("OrderID", vOrderID, allocator);

	rapidjson::Value vOrderType(kNumberType);
	vOrderType.SetInt(order);
	root.AddMember("OrderType", vOrderType, allocator);
	
	auto it = g_mapOrderParam.find(order);
	if (it == g_mapOrderParam.end())
	{
		CCAssert(false, "未设置命令参数格式");
		return "";
	}
	
	auto &vecOrderParam = it->second;
	for (auto itParam : vecOrderParam)
	{
		rapidjson::Value vElement(itParam.type);
		switch (itParam.type)
		{
		case kStringType:
			vElement.SetString(va_arg(listObject, string *)->c_str());
			break;
		case kNumberType:
			vElement.SetInt(*va_arg(listObject, int *));
			break;
		default:
			assert(false);
			va_arg(listObject, void *);
		}
		root.AddMember(itParam.name, vElement, allocator);
	}

	StringBuffer buffer;
	Writer<StringBuffer> writer(buffer);
	root.Accept(writer);
	std::string result = buffer.GetString();
	return result;
}