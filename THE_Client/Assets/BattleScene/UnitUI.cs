using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitUI
{
    private Unit unit;              // UI所属的单位
    public GameObject UnitImage;
    private GameObject textHP;
    private GameObject imageGroup;

    public UnitUI(Unit summonUnit, ChessboardPosition position)
    {
        if (Chessboard.GetCell(position).UnitOnCell == null)
        {
            unit = summonUnit;
            InitUnitImage();
            InitUnitGroupImage();
            InitUnitHPText();

            UnitManager.AddUnitSprite(this);
        }
    }

    private void InitUnitImage()
    {
        UnitImage = new GameObject();
        UnitImage.transform.SetParent(GameObject.Find("/Canvas/UnitImage").transform);
        Vector3 cellPosition = unit.curCell.GetLocalPosition();
        UnitImage.transform.localPosition = cellPosition;
        UnitImage.transform.localScale = new Vector3(Chessboard.CellSize / 75, Chessboard.CellSize / 75, 1);
        UnitImage.AddComponent<Image>().sprite = DataManager.CreateUnitSprite(unit.UnitAttribute.ID);
        UnitImage.AddComponent<RayIgnore>();
    }

    private void InitUnitGroupImage()
    {
        if (imageGroup == null)
        {
            imageGroup = new GameObject("UnitGroup");
            imageGroup.transform.SetParent(GameObject.Find("/Canvas/UnitGroup").transform);
            imageGroup.AddComponent<RectTransform>().pivot = new Vector2(1, 1);
            imageGroup.AddComponent<RayIgnore>();
            // 读取阵营标志的图片
            Sprite groupSprite = DataManager.CreateGroupSprite(unit.GroupType);

            imageGroup.transform.localScale = new Vector3(Chessboard.CellSize / groupSprite.texture.width / 64,
                Chessboard.CellSize / groupSprite.texture.width / 64,
                1);
            imageGroup.AddComponent<Image>().sprite = groupSprite;

            UpdateGroupPosition();
        }
    }

    /// <summary>
    /// 根据CurrentCell的位置更新阵营图标的位置
    /// </summary>
    public void UpdateGroupPosition()
    {
        Vector3 cellPosition = unit.curCell.GetLocalPosition();
        imageGroup.transform.localPosition = new Vector3(Chessboard.CellSize / 2 + cellPosition.x,
                Chessboard.CellSize / 2 + cellPosition.y,
                0);
    }

    public void UpdateGroup()
    {
        imageGroup.GetComponent<Image>().sprite = DataManager.CreateGroupSprite(unit.GroupType);
    }

    /// <summary>
    /// 更新unit左上显示HP的数字
    /// </summary>
    public void UpdateHP()
    {
        textHP.GetComponent<Text>().text = unit.HP.ToString();
    }

    public void SetGroupVisible(bool visible)
    {
        imageGroup.SetActive(visible);
    }

    private void InitUnitHPText()
    {
        if (textHP == null)
        {
            textHP = GameObject.Instantiate(GameObject.Find("UnitHPPrefab"));
            textHP.transform.SetParent(GameObject.Find("/Canvas/UnitHP").transform);
            textHP.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            textHP.GetComponent<Text>().text = unit.HP.ToString();
            UpdateHPPosition();
        }
    }

    public void UpdateHPPosition()
    {
        Vector3 cellPosition = unit.curCell.GetLocalPosition();
        textHP.transform.localPosition = new Vector3(-Chessboard.CellSize / 2 + cellPosition.x,
                Chessboard.CellSize / 2 + cellPosition.y,
                0);
    }

    public void SetHPVisible(bool visible)
    {
        textHP.SetActive(visible);
    }

    public void RemoveUnitSprite()
    {
        GameObject.Destroy(UnitImage);
        GameObject.Destroy(textHP);
        GameObject.Destroy(imageGroup);
    }

    public void SetPosition(Vector3 position)
    {
        UnitImage.transform.position = position;
    }
}