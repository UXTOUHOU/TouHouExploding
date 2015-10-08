#pragma once
#include "Community.h"

class CAck : public CCommunity
{
public:
	string ackID;
	virtual rapidjson::Value *MakeJsonValue() override;

	CAck();
	~CAck();
};