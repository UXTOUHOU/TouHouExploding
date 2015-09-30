#include "Respond.h"

CRespond::CRespond()
{
	className = "NetWork.Respond";
	needAck = false;
	netAttribute = 2;
	netMod = 1;
}

CRespond::~CRespond()
{

}

rapidjson::Value *CRespond::MakeJsonValue()
{
	auto value = CCommunity::MakeJsonValue();
	Document::AllocatorType& allocator = document.GetAllocator();
	value->AddMember("Error", error, allocator);
	value->AddMember("respondID", respondID.c_str(), allocator);
	return value;
}
