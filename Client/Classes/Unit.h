#pragma once
#include "cocos2d.h"
#include "ChessBoard.h"
#include "ChessBoardPosition.h"
#include "CardAttribute.h"
#include "ToolFunc.h"
USING_NS_CC;

enum UnitGroup
{
	UG_YOURSELF,
	UG_ENEMY,
};
class CCell;

class CUnit : public Sprite, public CCardAttribute
{
public:
	virtual bool init() override;
	CREATE_FUNC(CUnit);
	void InitCardAttribute(int unitID);
	void UnitDeath();
	void SetChessboardPosition(ChessboardPosition position);
	ChessboardPosition GetChessboardPosition() const;
	void SetMoveable(bool moveabel);
	bool GetMoveable() const;
	void SetCanAttack(bool canAttack);
	bool GetCanAttack() const;
	UnitGroup GetGroupType() const;
	Sprite *GetSpriteGroup() const;
	DrawNode *GetAttributeBackground() const;
	Label *GetLabelAttribute() const;
	int GetSkillNum() const;
	//单位左上角的HP显示
	Label *GetLabelHP() const;
	void SetHP(int HP);
	int GetHP() const;
	void ChangeHP(int deltaHP);
	void SetHPVisible(bool visible);
	void SetHPPosition(ChessboardPosition position);
	//单位右上角的阵营显示
	void SetGroup(UnitGroup groupType);
	void SetGroupVisible(bool visible);
	void SetGroupPosition(ChessboardPosition position);
	//鼠标悬停时的属性悬浮窗
	void SetAttribute(std::string attribute);
	void SetAttributeVisible(bool visible);
	void SetAttributePosition(Point position);

	CUnit();//int unitID, ChessboardPosition position
	~CUnit();
private:
	const float _attributeBorderWidth = 5;

	ChessboardPosition _chessboardPosition;

	int _HP;
	Label *_lblHP;
	UnitGroup _groupType;
	Sprite *_group;
	Label *_lblAttribute;
	DrawNode *_attributeBackground;

	bool _moveable;
	bool _canAttack;
};