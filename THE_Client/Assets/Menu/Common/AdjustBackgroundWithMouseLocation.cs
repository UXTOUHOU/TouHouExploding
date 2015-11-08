using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdjustBackgroundWithMouseLocation : MonoBehaviour {

    public GameObject AdjustedImage;
    public Canvas ParentCanvas;

    [Range(0f, 1f)]
    public float MinScale = 0.95f;
    public bool UpSideDown = false;
    public bool IsSmoothMoving = true;
    public float MaxSpeed = 70;
    public float BrakingDistance=300;
    public float MinSpeed = 0f;

    RectTransform rectTrans;
    Image image;
    float scale;

	// Use this for initialization
	void Start () {
        //获取Rect组件
        rectTrans = AdjustedImage.GetComponent<RectTransform>();
        //把锚点、中心位置固定在左上角
        rectTrans.anchorMax = new Vector2(0, 1);
        rectTrans.anchorMin = new Vector2(0, 1);
        rectTrans.pivot = new Vector2(0, 1);
        rectTrans.anchoredPosition = new Vector2(0, 0);

        //获取图片源
        image = AdjustedImage.GetComponent<Image>();
        //计算宽高缩放比
        float widthScale = image.canvas.pixelRect.width / image.mainTexture.width;
        float heightScale = image.canvas.pixelRect.height / image.mainTexture.height;
        //取最大值
        scale = widthScale > heightScale ? widthScale : heightScale;


    }
	

	// Update is called once per frame
	void Update () {
        //计算缩放限制
        float realScale = scale / MinScale;
        //调整图片大小
        var size = new Vector2(image.mainTexture.width * realScale,
                                            image.mainTexture.height * realScale);
        rectTrans.sizeDelta = size;

        //调整图片位置
        //获得鼠标位置
        var mPosition = Input.mousePosition;
        //检测是否越界，并把原点换为左上角
        mPosition = new Vector3(mPosition.x < image.canvas.pixelRect.width ?
                                                mPosition.x : image.canvas.pixelRect.width,
                                                mPosition.y < image.canvas.pixelRect.height ?
                                                image.canvas.pixelRect.height - mPosition.y : 0f);
        //换算为比例
        var posiScale = new Vector2(mPosition.x / image.canvas.pixelRect.width,
                                                mPosition.y / image.canvas.pixelRect.height);
        //计算多余大小
        var extraSize = size - new Vector2(image.canvas.pixelRect.width,
                                                               image.canvas.pixelRect.height);
        //转换为位置信息
        Vector2 picPosi;
        if(UpSideDown)
            picPosi = new Vector2( - extraSize.x * posiScale.x, extraSize.y * posiScale.y);
        else
            picPosi = new Vector2 (- extraSize.x * posiScale.x, extraSize.y * posiScale.y);
        //设定位置
        if ((!IsSmoothMoving) || MaxSpeed < 0 || MinSpeed < 0)
        {
            //如果设为0视为无此值
            rectTrans.anchoredPosition = picPosi;
        }
        else
        {
            //平滑移动
            SmoothMoveTo(picPosi, MaxSpeed, BrakingDistance, MinSpeed);




            //下面的逻辑呃……有点问题呢QAQ貌似是因为picPosi是实时在变的缘故吧【沉思
            //rectTrans.anchoredPosition = new Vector2
            //(Mathf.SmoothDamp(rectTrans.anchoredPosition.x, picPosi.x, ref currentSpeed, SmoothTime, MaxSpeed),
            //Mathf.SmoothDamp(rectTrans.anchoredPosition.y, picPosi.y, ref currentSpeed, SmoothTime, MaxSpeed));
        }
    }
    public void SmoothMoveTo(Vector2 target, float MaxSpeed, float brakingDistance, float minSpeed = 0f)
    {
        //计算速度
        float speed;
        float distance = (target - rectTrans.anchoredPosition).magnitude;
        if (distance >= brakingDistance)
        {
            speed = MaxSpeed;
        }
        else
        {
            speed = Mathf.Lerp(minSpeed, MaxSpeed, distance / brakingDistance);
        }
        //移动
        MoveTo(target, speed * Time.deltaTime);

    }
    /// <summary>
    /// 让目标物体向某个点移动
    /// </summary>
    /// <param name="target">目标位置</param>
    /// <param name="x">本次移动的距离</param>
    public void MoveTo(Vector2 target, float x = 0f)
    {
        if (x <= 0f)
        {
            rectTrans.anchoredPosition = target;
            return;
        }
        Vector2 direction = (target - rectTrans.anchoredPosition).normalized;
        float distance = (target - rectTrans.anchoredPosition).magnitude;
        //越界检测
        if (distance > x)
        {
            rectTrans.anchoredPosition += direction * x;
        }
        else
        {
            rectTrans.anchoredPosition = target;
        }
    }
}
