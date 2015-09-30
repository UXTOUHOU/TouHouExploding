#include "ToolFunc.h"

std::string WStrToUTF8(const std::wstring &src)
{
	std::string dest;
	for (size_t i = 0; i < src.size(); i++){
		wchar_t w = src[i];
		if (w <= 0x7f)
			dest.push_back((char)w);
		else if (w <= 0x7ff)
		{
			dest.push_back(0xc0 | ((w >> 6) & 0x1f));
			dest.push_back(0x80 | (w & 0x3f));
		}
		else if (w <= 0xffff)
		{
			dest.push_back(0xe0 | ((w >> 12) & 0x0f));
			dest.push_back(0x80 | ((w >> 6) & 0x3f));
			dest.push_back(0x80 | (w & 0x3f));
		}
		else if (sizeof(wchar_t) > 2 && w <= 0x10ffff)
		{
			//dest.push_back(0xf0 | ((w >> 18) & 0x07)); // wchar_t 4-bytes situation  
			dest.push_back(0x80 | ((w >> 12) & 0x3f));
			dest.push_back(0x80 | ((w >> 6) & 0x3f));
			dest.push_back(0x80 | (w & 0x3f));
		}
		else
			dest.push_back('?');
	}
	return dest;
}

char *GetUnitFileName(int unitID)
{
	static char buf[20];
	//返回 Unit_ID.png 格式的图片名
	strcpy(buf, "Unit_");
	char strID[10];
	_itoa(unitID, strID, 10);
	strcat(buf, strID);
	strcat(buf, ".png");
	return buf;
}

char *GetCardFileName(int cardID)
{
	static char buf[20];
	//返回 Card_ID.png 格式的图片名
	strcpy(buf, "Card_");
	char strID[10];
	_itoa(cardID, strID, 10);
	strcat(buf, strID);
	strcat(buf, ".png");
	return buf;
}
