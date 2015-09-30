#pragma once
#include <vector>
#include <map>
using namespace std;

class CCardAttribute
{
public:
	static map < int, CCardAttribute * > mapCardAttribute;
	static void InitMapCardAttribute(int cardID);

	int ID;				//卡片编号

	int hp;				//HP
	int motility;		//机动性
	int atk;			//攻击力
	int minAtkRange;	//攻击范围最小值
	int maxAtkRange;	//攻击范围最大值

	vector<wstring> vecSkillAttribute;
};
