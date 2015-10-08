#include "Community.h"

USING_NS_CC;

CCommunity::CCommunity()
{
	className = "NetWork.Community";
	needAck = false;
	netAttribute = 0;
	netMod = 1;
}

CCommunity::~CCommunity()
{
}

rapidjson::Value *CCommunity::MakeJsonValue()
{
	Document::AllocatorType& allocator = document.GetAllocator();
	rapidjson::Value *root = new rapidjson::Value(kObjectType);

	root->AddMember("ClassName", className.c_str(), allocator);	
	char szTime[100];
	static char szCommunityID[100];
	GetTime(szTime);
	strcpy(szCommunityID, szTime);
	strncat(szCommunityID, NewRandN(10, time(NULL)), 10);
	root->AddMember("CommunityID", szCommunityID, allocator);
	root->AddMember("NeedAck", needAck, allocator);
	root->AddMember("NetContent", netContent.c_str(), allocator);				//??
	root->AddMember("netAttribute", netAttribute, allocator);
	root->AddMember("netMod", netMod, allocator);
	root->AddMember("netTime", szTime, allocator);
	//////////////////////////////////////
	return root;
}

string CCommunity::ValueToJson(rapidjson::Value *value)
{
	if (value == NULL)
	{
		assert(false);
		return "";
	}
	StringBuffer buffer;
	Writer<StringBuffer> writer(buffer);
	value->Accept(writer);
	std::string result = buffer.GetString();
	delete value;
	value = NULL;
	return result;
}

const char *CCommunity::NewRandN(int len, time_t time) const
{
	static char randN[110];
	static int n = 0;
	srand(time);
	randN[0] = '\0';
	char buf[10];
	if (len > 100)
		assert(false);
	int nowLen = 0;
	while (nowLen++ < len)
	{
		strcat(randN, _itoa(rand() % 10, buf, 10));
	}
	return randN;
}

void CCommunity::GetTime(char *szTime)
{
	tm *tmTime;
	time_t nowTime = time(NULL);
	tmTime = localtime(&nowTime);
	strftime(szTime, 100, "%y%m%d%H%M%S", tmTime);
	//ºÁÃë
	char szMS[4];
	int ms = nowTime % 1000;
	for (int i = 2; i >= 0; --i)
	{
		szMS[i] = ms % 10 + '0';
		ms /= 10;
	}
	szMS[3] = '\0';
	strncat(szTime, szMS, sizeof(szMS));
}