#pragma once
#include <cocos2d.h>
#include <WinSock2.h>
#include <time.h>
#include "json\document.h"
#include "json\rapidjson.h"
#include "json\stringbuffer.h"
#include "json\writer.h"

using namespace std;
using namespace rapidjson;
class CCommunity
{
public:
	Document document;

	string className;
	bool needAck;
	string netContent;
	int netAttribute;
	int netMod;
	string netTime;

	virtual rapidjson::Value *MakeJsonValue();
	string ValueToJson(rapidjson::Value *value);

	const char *NewRandN(int len, time_t time) const;	//生成新的10位随机数
	void GetTime(char *buf);

	CCommunity();
	~CCommunity();
};

