#include "CardDetail.h"

CCardDetail *CCardDetail::pCardDetail = NULL;

CCardDetail::CCardDetail()
{

}

CCardDetail::~CCardDetail()
{

}

SceneType CCardDetail::Enter()
{
	setVisible(true);
	return sceneCardDetail;
}