#include "CPVPConnect.h"
#include "CPVPMode.h"
#include "json\rapidjson.h"
#include "json\stringbuffer.h"
#include "json\writer.h"
#include <iostream>

using namespace rapidjson;

CPVPConnect *CPVPConnect::pPVPConnect = NULL;

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
	//
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(1, 1), &wsa) != 0)
	{
		CCASSERT(false, "socket¼ÓÔØÊ§°Ü");
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

void CPVPConnect::login(string playerName, string passWord)
{
	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_Login),
		&JsonElement("PlayerName", kStringType, &playerName),
		&JsonElement("PassWord", kStringType, &passWord), 
		NULL));
}

void CPVPConnect::versionCheck(int version)
{
	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_VersionCheck),
		&JsonElement("ClientVersion", kNumberType, version),
		NULL));
}

void CPVPConnect::unitMove(int x, int y)
{
	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_UnitMove),
		&JsonElement("PositionX", kNumberType, x),
		&JsonElement("PositionY", kNumberType, y),
		NULL));
}

void CPVPConnect::unitAttack(int x, int y)
{
	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_UnitAttack),
		&JsonElement("PositionX", kNumberType, x),
		&JsonElement("PositionY", kNumberType, y),
		NULL));
}

void CPVPConnect::useCard(int cardID)
{
	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_UseCard),
		&JsonElement("CardID", kNumberType, cardID),
		NULL));
}

void CPVPConnect::useSkill(int skillID)
{
	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_UseSkill),
		&JsonElement("CardID", kNumberType, skillID),
		NULL));
}

void CPVPConnect::summon(int unitID)
{
	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_Summon),
		&JsonElement("CardID", kNumberType, unitID),
		NULL));
}

void CPVPConnect::endTurn()
{
	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_EndTurn),
		NULL));
}

void CPVPConnect::askRoomList()
{
	_sendToServer(_makeJsonString(&JsonElement("Order", kNumberType, OT_CS_AskRoomList),
		NULL));
}

string CPVPConnect::_makeJsonString(JsonElement *element, ...)
{
	va_list elementList;
	va_start(elementList, element);

	Document document;
	Document::AllocatorType& allocator = document.GetAllocator();
	rapidjson::Value root(kObjectType);

	rapidjson::Value vOrderID(kNumberType);
	vOrderID.SetInt(++_nowOrderID);
	root.AddMember("OrderID", vOrderID, allocator);

	JsonElement *it = element;
	do
	{
		if (it == NULL)
			break;
		assert(it->information.pvoid != NULL && it->name != NULL);

		rapidjson::Value vElement(it->type);
		switch (it->type)
		{
		case kStringType:
			vElement.SetString(it->information.pstr->c_str());
			break;
		case kNumberType:
			vElement.SetInt(it->information.num);
			break;
		default:
			assert(false);
		}
		root.AddMember(it->name, vElement, allocator);
	}while (it = va_arg(elementList, JsonElement *));

	StringBuffer buffer;
	Writer<StringBuffer> writer(buffer);
	root.Accept(writer);
	std::string result = buffer.GetString();
	return result;
}