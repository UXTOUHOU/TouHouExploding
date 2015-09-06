#include "CPVPConnect.h"
#include "CPVPMode.h"
#include "json\rapidjson.h"
#include "json\stringbuffer.h"
#include "json\writer.h"
#include <iostream>
using namespace rapidjson;

CPVPConnect::CPVPConnect()
{
}

CPVPConnect::~CPVPConnect()
{
}

void CPVPConnect::init()
{
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(1, 1), &wsa) != 0)
	{
		CCASSERT(false, "socketº”‘ÿ ß∞‹");
		return;
	}
	serverAddr.sin_family = AF_INET;
	serverAddr.sin_port = 2333;
	serverAddr.sin_addr.s_addr = htonl(INADDR_LOOPBACK);

	clientSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	int ret = connect(clientSocket, (sockaddr *)&serverAddr, sizeof(serverAddr));
	log("connect ret", ret);

	//test
	login("", "");
	//
}

void CPVPConnect::_sendToServer(std::string str)
{
	::send(clientSocket, str.c_str(), sizeof(str.length()), 0);
}

void CPVPConnect::login(string playerName, string passWord)
{
	//Document document;
	//Document::AllocatorType& allocator = document.GetAllocator();
	//rapidjson::Value root(kObjectType);

	//rapidjson::Value 
}