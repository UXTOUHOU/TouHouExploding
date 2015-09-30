#pragma once
#include "Community.h"

class CRespond : public CCommunity
{
public:
	void *error;
	string respondID;
	virtual rapidjson::Value *MakeJsonValue() override;

	CRespond();
	~CRespond();
};