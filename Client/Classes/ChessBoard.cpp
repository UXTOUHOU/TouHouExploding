#include "Chessboard.h"

CLayerChessboard::CLayerChessboard() :
	_chessboardPosition(446.40F, 241.11F),
	playerState(PS_Wait),
	_currentCell(NULL),
	selectedCell(NULL),
	prevCursorChessboardX(-1),
	prevCursorChessboardY(-1),
	_skillDescribe(this, g_floatingWindowZOrder)
{
	memset(_cell, NULL, sizeof(_cell));
}

CLayerChessboard::~CLayerChessboard()
{
}

bool CLayerChessboard::init()
{
	Layer::init();
	//棋盘背景
	SpriteBatchNode *batchNode = SpriteBatchNode::create("temp.png");
	for (int y = 0; y < 8; ++y)
		for (int x = 0; x < 12; ++x)
		{
			//cell init
			_cell[y][x] = new CCell;
			auto &cell = _cell[y][x];
			cell->cellPosition = GetChessboardPosition(ChessboardPosition(x, y));
			cell->chessboardPosition.SetPosition(x, y);
			//cell->unitState.state = CCell::UnitState::ST_Movable;
			//格子背景
			cell->backGround = Sprite::createWithTexture(batchNode->getTexture());
			batchNode->addChild(cell->backGround);
			cell->backGround->setColor(Color3B::BLACK);
			ShowMapCell(cell->backGround, x, y);
		}
	addChild(batchNode, -1);
	//棋盘网格
	DrawNode *drawNode = DrawNode::create();
	Point position = _chessboardPosition;
	for (int i = 0; i < 13; ++i)
	{
		drawNode->drawLine(position, position + Vec2(0, 600), Color4F::WHITE);
		position.x += _spacingX;
	}
	position = _chessboardPosition;
	for (int i = 0; i < 9; ++i)
	{
		drawNode->drawLine(position, position + Vec2(900, 0), Color4F::WHITE);
		position.y += _spacingY;
	}
	addChild(drawNode, 0);
	////添加属性悬浮框
	//for (int y = 0; y < 1; ++y)
	//	for (int x = 0; x < 12; ++x)
	//	{
	//		auto &cell = _cell[y][x];
	//		cell->unit->SetAttribute(WStrToUTF8(L"名字: ...\n血量: 0\n机动: 0\n攻击: 0\n射程: 0\n技能: ..."));
	//		addChild(cell->unit->GetAttributeBackground(), 1);
	//		addChild(cell->unit->GetLabelAttribute(), 1);
	//	}
	////
	////技能浮窗
	//InitFloatingWindow(_lblSkillAttribute, _drawNodeSkillAttribute, this);
	////_lblSkillAttribute = Label::create();
	////_lblSkillAttribute->setVisible(false);
	////_drawNodeSkillAttribute = DrawNode::create();
	////_drawNodeSkillAttribute->setVisible(false);
	//////addChild(_lblSkillAttribute, g_floatingWindow);
	////addChild(_drawNodeSkillAttribute, g_floatingWindow);
	////_drawNodeSkillAttribute->addChild(_lblSkillAttribute);
	//显示选择的格子
	//
	auto listenerMouse = EventListenerMouse::create();
	listenerMouse->onMouseMove = [this](Event *event) //std::bind(&CLayerChessboard::OnMouseMove, this, std::placeholders::_1); 
	{
		auto eventMouse = (EventMouse *)event;
		OnMouseMove(eventMouse->getLocationInView());
	};
	listenerMouse->onMouseDown = [this](Event *event)
	{
		auto eventMouse = (EventMouse *)event;
		ChessboardPosition position;
		GetCellNum(position, eventMouse->getLocationInView());
		if (position.x != -1)
		{
			prevLeftButtonDownX = position.x;
			prevLeftButtonDownY = position.y;
			leftButtonDown = true;
		}
		log("mouse down");
	};
	listenerMouse->onMouseUp = [this](Event *event)
	{
		auto eventMouse = (EventMouse *)event;
		ChessboardPosition position;
		GetCellNum(position, eventMouse->getLocationInView());
		//auto cell = position.GetCell();
		if (position.x != -1 &&
			prevLeftButtonDownX == position.x &&
			prevLeftButtonDownY == position.y)
		{
			if (eventMouse->getMouseButton() == 0 && 
				leftButtonDown)
				//按下了左键
				OnClickCell(position.x, position.y);
			else
				//按下了右键
				switch (playerState)
				{
				case PS_Wait:
				case PS_WaitingOperate:
				case PS_WaitMoveAnimateEnd:
				case PS_WaitAttackAnimateEnd:
					break;
				case PS_SelectUnitBehavior:
				{//取消操作时让unit重新执行movable的动画
					auto &cell = selectedCell;
					float t = abs((cell->unit->getPosition().y - cell->cellPosition.y) / 5);	
					if (cell->unit->GetGroupType() == UG_YOURSELF)
					{
						if (cell->unit->GetMoveable())
							cell->unit->runAction(Sequence::create(MoveTo::create(t, cell->cellPosition),
								CallFunc::create([cell](){cell->unit->runAction(CCell::Moveable()); }),
								NULL));
					}
				}
					selectedCell = NULL;
					ClearBackGround();
					HideOperateButton();
					ChangeState(PS_WaitingOperate);
					break;
				case PS_SelectMovePosition:
					ChangeState(PS_SelectUnitBehavior);
					break;
				case PS_SelectAttackTarget:
					ChangeState(PS_SelectUnitBehavior);
					break;
				case PS_RunningSkill:
					//ChangeState(PS_SelectSkill);
					break;
				case PS_SelectSkill:
					ChangeState(PS_SelectUnitBehavior);
					break;
				default:
					assert(false);
				}
		}
		leftButtonDown = false;
	};

	_eventDispatcher->addEventListenerWithFixedPriority(listenerMouse, -10);

	auto listenerKeyboard = EventListenerKeyboard::create();
	listenerKeyboard->onKeyReleased = [this](EventKeyboard::KeyCode keyCode, Event *event)
	{
		switch (keyCode)
		{
		case EventKeyboard::KeyCode::KEY_KP_ENTER:
			if (playerState == PS_SelectMovePosition)
			{
				if (!startRecordMovePath) return;
				//listMovePath.pop_front();
				selectedCell->MoveWithPath(listMovePath);
			}
			break;
		case EventKeyboard::KeyCode::KEY_UP_ARROW:
		{
			ChessboardPosition position = *this->listMovePath.rbegin();
			if (listMovePath.empty())
				position = selectedCell->chessboardPosition;
			position.y += 1;
			startRecordMovePath = true;
			RecordMovePath(position);
		}
			break;
		case EventKeyboard::KeyCode::KEY_DOWN_ARROW:
		{
			ChessboardPosition position = *this->listMovePath.rbegin();
			if (listMovePath.empty())
				position = selectedCell->chessboardPosition;
			position.y -= 1;
			startRecordMovePath = true;
			RecordMovePath(position);
		}
			break;
		case EventKeyboard::KeyCode::KEY_LEFT_ARROW:
		{
			ChessboardPosition position = *this->listMovePath.rbegin();
			if (listMovePath.empty())
				position = selectedCell->chessboardPosition;
			position.x -= 1;
			startRecordMovePath = true;
			RecordMovePath(position);
		}
			break;
		case EventKeyboard::KeyCode::KEY_RIGHT_ARROW:
		{
			ChessboardPosition position = *this->listMovePath.rbegin();
			if (listMovePath.empty())
				position = selectedCell->chessboardPosition;
			position.x += 1;
			startRecordMovePath = true;
			RecordMovePath(position);
		}
			break;
		default:
			break;
		}
	};
	_eventDispatcher->addEventListenerWithSceneGraphPriority(listenerKeyboard, this);
	//operateButton
	buttonMove = ui::Button::create("Move_Normal.png", "Move_Press.png", "Move_Disable.png");
	//buttonMove->setContentSize(Size(60, 60));
	//buttonMove->setTitleFontSize(18);
	//buttonMove->setTitleText(WStrToUTF8(L"移动"));

	buttonAttack = ui::Button::create("Attack_Normal.png", "Attack_Press.png", "Attack_Disable.png");
	//buttonAttack->setContentSize(Size(60, 60));
	//buttonAttack->setTitleFontSize(18);
	//buttonAttack->setTitleText(WStrToUTF8(L"攻击"));

	buttonSkill = ui::Button::create("Skill_3_Normal.png", "Skill_3_Press.png", "Skill_3_Disable.png");
	//buttonSkill->setContentSize(Size(60, 60));
	//buttonSkill->setTitleFontSize(18);
	//buttonSkill->setTitleText(WStrToUTF8(L"技能"));

	buttonMove->setVisible(false);
	buttonAttack->setVisible(false);
	buttonSkill->setVisible(false);

	buttonMove->addClickEventListener(CC_CALLBACK_0(CLayerChessboard::OnButtonMove, this));
	buttonAttack->addClickEventListener(CC_CALLBACK_0(CLayerChessboard::OnButtonAttack, this));
	buttonSkill->addClickEventListener(CC_CALLBACK_0(CLayerChessboard::OnButtonSkill, this));

	buttonSkill_1 = ui::Button::create("Skill_1_Normal.png", "Skill_1_Press.png", "Skill_1_Disable.png");
	//buttonSkill_1->setTitleFontSize(18);
	//buttonSkill_1->setTitleText(WStrToUTF8(L"技能1"));

	buttonSkill_2 = ui::Button::create("Skill_2_Normal.png", "Skill_2_Press.png", "Skill_2_Disable.png");
	//buttonSkill_2->setTitleFontSize(18);
	//buttonSkill_2->setTitleText(WStrToUTF8(L"技能2"));
	buttonSkill_1->setVisible(false);
	buttonSkill_2->setVisible(false);

	buttonSkill_1->addClickEventListener(CC_CALLBACK_0(CLayerChessboard::OnButtonSkill_1, this));
	buttonSkill_2->addClickEventListener(CC_CALLBACK_0(CLayerChessboard::OnButtonSkill_2, this));

	addChild(buttonMove, 50);
	addChild(buttonAttack, 50);
	addChild(buttonSkill, 50);
	addChild(buttonSkill_1, 50);
	addChild(buttonSkill_2, 50);
	//
	//ChessboardDialog
	_dialogNode = CSLoader::createNode("ChessboardDialog.csb");
	auto pannel = _dialogNode->getChildByName<ui::Layout *>("Panel_Dialog");
	_dialogText = pannel->getChildByName<ui::Text *>("Text_Describe");
	_dialogYes = pannel->getChildByName<ui::Button *>("Button_Yes");
	_dialogNo = pannel->getChildByName<ui::Button *>("Button_No");

	_dialogYes->addClickEventListener(CC_CALLBACK_0(CLayerChessboard::_OnButtonDialogYes, this));
	_dialogNo->addClickEventListener(CC_CALLBACK_0(CLayerChessboard::_OnButtonDialogNo, this));

	_dialogNode->setVisible(false);
	addChild(_dialogNode);
	//
	//Skill
	SkillOperate::AddEventSkillEndListener([](void *)
	{
		ChangeState(PS_SelectUnitBehavior);
	});
	//
	return true;
}

void CLayerChessboard::SetUnit(int unitID, int x, int y)
{
	_cell[y][x]->SetUnit(unitID, ChessboardPosition(x, y));
	addChild(_cell[y][x]->unit);
}

void CLayerChessboard::ShowMapCell(Sprite *sprite, int x, int y)
{
	sprite->setPosition(GetChessboardPosition(ChessboardPosition(x, y)));
}

void CLayerChessboard::DelUnit(ChessboardPosition position)
{
	_cell[position.y][position.x]->unit->UnitDeath();
	_cell[position.y][position.x]->unit = NULL;
}

Point CLayerChessboard::GetChessboardPosition(ChessboardPosition position)
{
	Point point = _chessboardPosition;
	point.x += (position.x + 0.5F) * _spacingX;
	point.y += (position.y + 0.5F) * _spacingY;
	return point;
}

void CLayerChessboard::GetCellNum(ChessboardPosition &resPosition, Point cursor)
{
	Point position = _chessboardPosition;
	float fx = (cursor.x - position.x) / _spacingX;
	float fy = (cursor.y - position.y) / _spacingY;
	resPosition.x = fx;
	resPosition.y = fy;
	if (fx >= 0 && fy >= 0 &&	//防止负数转换时被近似成0
		0 <= resPosition.x && resPosition.x < 12 &&
		0 <= resPosition.y && resPosition.y < 8)
	{
		return;
	}
	else{
		resPosition.x = -1;
		resPosition.y = -1;
	}
}

void CLayerChessboard::HoverSkillButton(Point cursorPosition)
{
	if (GetButtonRect(buttonSkill_1).containsPoint(cursorPosition))
	{
		_currentSkillID = 1;
	}
	else if (GetButtonRect(buttonSkill_2).containsPoint(cursorPosition))
	{
		_currentSkillID = 2;
	}
	else
	{
		_currentSkillID = -1;
		//unschedule(schedule_selector(CLayerChessboard::ShowSkillDescribe));
		HideSkillDescribe();
		return;
	}
	//schedule(schedule_selector(CLayerChessboard::ShowSkillDescribe), 1.F);
	ShowSkillDescribe(0.F);
	SetSkillDescribePosition(cursorPosition);

	//else if (GetButtonRect(buttonSkill_3).containsPoint(cursorPosition))
	//{
	//	_currentSkillID = 3;
	//	schedule(schedule_selector(CLayerChessboard::ShowAttribut), 1.F);
	//}

}

void CLayerChessboard::OnMouseMove(Point cursorPosition)
{
	//Point cursorPosition = ((EventMouse *)event)->getLocationInView();
	prevPointCursor = cursorPosition;
	ChessboardPosition position;
	GetCellNum(position, cursorPosition);
	//
	if (playerState == PS_SelectSkill)
	{
		HoverSkillButton(cursorPosition);
	}
	//
	if (position.x != -1)
	{
		//鼠标在棋盘内的移动
		if (_currentCell && _currentCell->unit)
		{
			_currentCell->unit->SetAttributePosition(cursorPosition);
		}

		if (prevCursorChessboardX != position.x ||
			prevCursorChessboardY != position.y)
		{
			//改变了格子
			switch (playerState)
			{
			case PS_Wait:
			case PS_WaitMoveAnimateEnd:
			case PS_WaitAttackAnimateEnd:
			case PS_SelectSkill:
				break;
			case PS_WaitingOperate:
				ShowSelectCell(position.x, position.y);
				break;
			case PS_SelectMovePosition:
				RecordMovePath(position);
				break;
			case PS_SelectUnitBehavior:
				break;
			case PS_SelectAttackTarget:
				ClearBackGround();
				ShowAtkRange(selectedCell->chessboardPosition, 3);
				if (position.InRange(selectedCell->chessboardPosition, 3))
					ShowAtkCell(position);
				break;
			case PS_RunningSkill:

				break;
			//case PS_SelectSkillTarget:
			//	ClearBackGround();
			//	ShowAtkRange(selectedCell->chessboardPosition, 3);
			//	if (position.InRange(selectedCell->chessboardPosition, 3))
			//		ShowAtkCell(position);
			//	break;
			default:
				assert(false);
			}

			prevCursorChessboardX = position.x;
			prevCursorChessboardY = position.y;
			log("change cell");
			unschedule(schedule_selector(CLayerChessboard::ShowAttribut));
			if (_currentCell && _currentCell->unit)
				_currentCell->unit->SetAttributeVisible(false);
			_currentCell = GetCell(position);
			schedule(schedule_selector(CLayerChessboard::ShowAttribut), 1.F);
		}
		//
		log("Chessboard %d %d", position.x, position.y);
	}
	else{
		//移出了棋盘
		prevCursorChessboardX = -1;
		prevCursorChessboardY = -1;
		unschedule(schedule_selector(CLayerChessboard::ShowAttribut));
		if (_currentCell && _currentCell->unit)
			_currentCell->unit->SetAttributeVisible(false);
		switch (playerState)
		{
		case PS_Wait:
		case PS_WaitingOperate:
			ClearBackGround();
			break;
		case PS_SelectAttackTarget:
			ClearBackGround();
			ShowAtkRange(selectedCell->chessboardPosition, 3);
			break;
		default:
			break;
		}
		log("out chellboard");
	}
}

void CLayerChessboard::SelectOperateUnit(ChessboardPosition position)
{
	auto cell = GetCell(position);
	if (cell->unit != NULL &&
		cell->unit->GetGroupType() == UG_YOURSELF)
	{
		cell->unit->stopAllActions();
		ShowSelectCell(position.x, position.y);
		selectedCell = GetCell(position);
		ChangeState(PS_SelectUnitBehavior);
	}
}

void CLayerChessboard::OnClickCell(int x, int y)
{
	auto targetPosition = ChessboardPosition(x, y);
	switch (playerState)
	{
	case PS_Wait:
	case PS_WaitMoveAnimateEnd:
	case PS_WaitAttackAnimateEnd:
		break;
	case PS_SelectSkill:
		SelectOperateUnit(targetPosition);
		break;
	case PS_RunningSkill:
		SkillOperate::mutexSkill.lock();
		if (SkillOperate::bWaitSelectCell)
		{
			SkillOperate::cellSkillTarget = GetCell(targetPosition);
			SkillOperate::bWaitSelectCell = false;
		}
		SkillOperate::mutexSkill.unlock();
		break;
	case PS_WaitSelectCell:
		SkillOperate::mutexSkill.lock();
		SkillOperate::cellSkillTarget = GetCell(targetPosition);
		SkillOperate::mutexSkill.unlock();
		break;
	//case PS_SelectSkillTarget:
	//	SkillOperate::mutexSkill.lock();
	//	SkillOperate::g_cellSkillTarget = GetCell(targetPosition);
	//	SkillOperate::mutexSkill.unlock();
	//	//UseSkill(skillID, target);
	//	break;
	case PS_WaitingOperate:
		SelectOperateUnit(targetPosition);
		break;
	case PS_SelectUnitBehavior:
		SelectOperateUnit(targetPosition);
		break;
	case PS_SelectMovePosition:
		if (!listMovePath.empty() &&
			targetPosition == *listMovePath.rbegin())
		{
			ChangeState(PS_WaitMoveAnimateEnd);
			selectedCell->MoveWithPath(listMovePath);
		}
		break;
	case PS_SelectAttackTarget:
	{
		if (!targetPosition.InRange(selectedCell->GetCellNum(), 3)) break;
		auto cell = GetCell(targetPosition);
		if (cell->unit &&							//目标地点有单位
			cell->unit->GetGroupType() == UG_ENEMY)	//是敌方单位
		{
			ChangeState(PS_WaitAttackAnimateEnd);
			cell->unit->runAction(Sequence::create((ActionInterval *)CCell::BeAttacked(),
				CallFunc::create([cell, this]()
				{
					cell->unit->ChangeHP(-this->selectedCell->unit->atk);
					if (cell->unit->GetHP() == 0)
					{
						cell->unit->UnitDeath();
						cell->unit = NULL;
					}
					else
						this->selectedCell->unit->SetCanAttack(false);
					ChangeState(PS_SelectUnitBehavior); 
				}),
				NULL));
		}
	}
		break;
	default:
		assert(false);
	}
}

void CLayerChessboard::ShowMovableRange(ChessboardPosition position, int range)
{
	ClearBackGround();

	for (int i = MAX(0, position.y - range); i < MIN(CHESSBOARD_MAX_Y, position.y + range + 1); ++i)
		for (int j = MAX(0, position.x - range); j < MIN(CHESSBOARD_MAX_X, position.x + range + 1); ++j)
		{	
			if (abs(position.y - i) + abs(position.x - j) <= range)
			{
				_cell[i][j]->backGround->setColor(Color3B(70, 112, 70));
			}
		}
}

void CLayerChessboard::ShowSelectUnitSkillList()
{
	int skillNum = selectedCell->unit->GetSkillNum();
	if (skillNum == 1)
	{
		buttonSkill_1->setPosition(selectedCell->cellPosition + Point(0, 50));
		buttonSkill_1->setVisible(true);
	}
	else if (skillNum == 2){
		buttonSkill_1->setPosition(selectedCell->cellPosition + Point(-40, 40));
		buttonSkill_1->setVisible(true);
		buttonSkill_2->setPosition(selectedCell->cellPosition + Point(40, 40));
		buttonSkill_2->setVisible(true);
	}
	else// if (skillNum == 3)
		return;//assert(false);
}

void CLayerChessboard::HideSelectUnitSkillList()
{
	buttonSkill_1->setVisible(false);
	buttonSkill_2->setVisible(false);
}

void CLayerChessboard::ShowAtkRange(ChessboardPosition position, int range)
{
	ClearBackGround();

	for (int i = MAX(0, position.y - range); i < MIN(CHESSBOARD_MAX_Y, position.y + range + 1); ++i)
		for (int j = MAX(0, position.x - range); j < MIN(CHESSBOARD_MAX_X, position.x + range + 1); ++j)
		{
			if (abs(position.y - i) + abs(position.x - j) <= range)
			{
				_cell[i][j]->backGround->setColor(Color3B(149, 70, 70));
			}
		}
}

void CLayerChessboard::ShowSelectCell(int x, int y)
{//显示被选择的效果
	ClearBackGround();
	_cell[y][x]->backGround->setColor(Color3B(60, 81, 102));
}

void CLayerChessboard::ShowAtkCell(ChessboardPosition position)
{//显示攻击的目标单位的效果
	_cell[position.y][position.x]->backGround->setColor(Color3B(169, 90, 90));
}

CCell *CLayerChessboard::GetCell(ChessboardPosition position)
{
	return _cell[position.y][position.x];
}

void CLayerChessboard::ClearBackGround()
{
	for (int i = 0; i < 8; ++i)
		for (int j = 0; j < 12; ++j)
		{
			_cell[i][j]->backGround->setColor(Color3B(0, 0, 0));
			_cell[i][j]->backGround->setOpacity(255);
		}
}

void CLayerChessboard::ShowAttribut(float dt)
{
	if (_currentCell && _currentCell->unit)
	{
		_currentCell->unit->SetAttributePosition(prevPointCursor);
		_currentCell->unit->SetAttributeVisible(true);
	}
}

void CLayerChessboard::HideOperateButton()
{
	buttonMove->setVisible(false);
	buttonAttack->setVisible(false);
	buttonSkill->setVisible(false);
}

void CLayerChessboard::ShowOperateButton()
{
	if (selectedCell == NULL)
	{
		assert(false);
		return;
	}
	buttonMove->setPosition(selectedCell->cellPosition + Point(-40, 20));
	buttonAttack->setPosition(selectedCell->cellPosition + Point(0, 50));
	buttonSkill->setPosition(selectedCell->cellPosition + Point(40, 20));
	

	buttonMove->setVisible(true);
	buttonMove->setEnabled(selectedCell->unit->GetMoveable());
	buttonMove->setBright(selectedCell->unit->GetMoveable());
	buttonAttack->setVisible(true);
	buttonAttack->setEnabled(selectedCell->unit->GetCanAttack());
	buttonAttack->setBright(selectedCell->unit->GetCanAttack());
	buttonSkill->setVisible(true);
	//
	//
}

void CLayerChessboard::OnButtonMove()
{
	if (playerState != PS_SelectUnitBehavior)
		return;
	prevLeftButtonDownX = -1;				//防止按钮的click事件继续被捕捉
	ChangeState(PS_SelectMovePosition);
	HideOperateButton();
}

void CLayerChessboard::OnButtonAttack()
{
	if (playerState != PS_SelectUnitBehavior)
		return;
	prevLeftButtonDownX = -1;				//防止按钮的click事件继续被捕捉
	ChangeState(PS_SelectAttackTarget);
	HideOperateButton();
}

void CLayerChessboard::OnButtonSkill()
{
	if (playerState != PS_SelectUnitBehavior)
		return;
	prevLeftButtonDownX = -1;				//防止按钮的click事件继续被捕捉
	ChangeState(PS_SelectSkill);
	HideOperateButton();
}

void CLayerChessboard::OnButtonSkill_1()
{
	if (playerState != PS_SelectSkill)
		return;
	prevLeftButtonDownX = -1;				//防止按钮的click事件继续被捕捉
	//ChangeState(PS_SelectSkillTarget);
	ChangeState(PS_RunningSkill);
	//test
	static CSkill_1 *skill = new CSkill_1;
	skill->RunSkill();
	//if (skill->RunSkill())
	//{
	//	skill = NULL;
	//	ChangeState(PS_SelectUnitBehavior);
	//}
	//
	HideSelectUnitSkillList();
}

void CLayerChessboard::OnButtonSkill_2()
{
	if (playerState != PS_SelectSkill)
		return;
	prevLeftButtonDownX = -1;				//防止按钮的click事件继续被捕捉
	//ChangeState(PS_SelectSkillTarget);
	ChangeState(PS_RunningSkill);
	HideSelectUnitSkillList();
}

void CLayerChessboard::RecordMovePath(ChessboardPosition position)
{
	if (!position.OnChessboard() ||				//记录点在棋盘外
		playerState != PS_SelectMovePosition ||	//非记录路径状态
		(GetCell(position)->unit != NULL && GetCell(position) != selectedCell))		//地点已有单位且非选中的单位
		return;
	if (!startRecordMovePath)
	{
		//未开始记录路径
		auto originPosition = selectedCell->GetCellNum();
		if (position == originPosition)
			startRecordMovePath = true;
	}
	else{
		auto originPosition = selectedCell->GetCellNum();
		auto lastPosition = *listMovePath.rbegin();
		if ((!listMovePath.empty() ? position.Adjacent(lastPosition) : position.Adjacent(originPosition)) && 	//与上一个格子相邻
			originPosition.InRange(position, 3))																//在移动范围内
		{
			//可移动的位置
			GetCell(position)->backGround->setColor(Color3B(100, 142, 100));
			listMovePath.push_back(position);
		}
		else{
			//不可移动的位置
			return;
		}
	}
}

void CLayerChessboard::SetDialogString(string str)
{
	_dialogText->setString(str);
}

void CLayerChessboard::SetDialogVisible(bool visible)
{
	_dialogNode->setVisible(visible);
}

Rect GetButtonRect(ui::Button *button)
{//不计算缩放
	Rect buttonRect;
	auto anchorPoint = button->getAnchorPoint();
	auto size = button->getContentSize();
	buttonRect.origin = button->getPosition() - Point(anchorPoint.x * size.width, anchorPoint.y * size.height);
	buttonRect.size = size;
	return buttonRect;
}

void CLayerChessboard::SetSkillDescribePosition(Point point)
{
	_skillDescribe.SetPosition(point);
}

void CLayerChessboard::ShowSkillDescribe(float dt)
{
	auto str = selectedCell->unit->vecSkillAttribute[_currentSkillID - 1];
	_skillDescribe.SetString(WStrToUTF8(LineBreak(str, 20)));
	_skillDescribe.SetVisible(true);
	//UpdateFloatingWindowString(_lblSkillAttribute, _drawNodeSkillAttribute, WStrToUTF8(str), 5);
	//_lblSkillAttribute->setVisible(true);
	//_drawNodeSkillAttribute->setVisible(true);
	//log("ShowSkillDescribe");
}

void CLayerChessboard::HideSkillDescribe()
{
	_skillDescribe.SetVisible(false);
}

wstring LineBreak(const wstring &wstr, int n)
{
	wstring result;

	auto it = wstr.begin();
	for (int i = 1; it != wstr.end(); ++it, ++i)
	{
		result.push_back(*it);
		if (i % n == 0)
			result.push_back(L'\n');
	}
	return result;
}


void CLayerChessboard::_OnButtonDialogYes()
{
	SkillOperate::mutexSkill.lock();
	SkillOperate::bClickDialogButton = true;
	SkillOperate::bDialogReturn = true;
	SkillOperate::mutexSkill.unlock();
}	

void CLayerChessboard::_OnButtonDialogNo()
{
	SkillOperate::mutexSkill.lock();
	SkillOperate::bClickDialogButton = true;
	SkillOperate::bDialogReturn = false;
	SkillOperate::mutexSkill.unlock();
}
