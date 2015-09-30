#include "Ack.h"

CAck::CAck()
{
	className = "NetWork.Ack";
	needAck = false;
	netAttribute = 2;
	netMod = 1;
}

CAck::~CAck()
{

}

rapidjson::Value *CAck::MakeJsonValue()
{
	auto value = CCommunity::MakeJsonValue();
	Document::AllocatorType& allocator = document.GetAllocator();
	value->AddMember("ackID", ackID.c_str(), allocator);
	return value;
}
