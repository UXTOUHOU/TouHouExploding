#include "AppDelegate.h"
#include "SceneMenu.h"
#include "cocos-ext.h"
#include "ui\CocosGUI.h"
#include "cocostudio\CocoStudio.h"

USING_NS_CC;
USING_NS_CC_EXT;

AppDelegate::AppDelegate() {

}

AppDelegate::~AppDelegate() 
{
}

//if you want a different context,just modify the value of glContextAttrs
//it will takes effect on all platforms
void AppDelegate::initGLContextAttrs()
{
    //set OpenGL context attributions,now can only set six attributions:
    //red,green,blue,alpha,depth,stencil
    GLContextAttrs glContextAttrs = {8, 8, 8, 8, 24, 8};

    GLView::setGLContextAttrs(glContextAttrs);
}

bool AppDelegate::applicationDidFinishLaunching() {
    // initialize director
    auto director = Director::getInstance();
    auto glview = director->getOpenGLView();
    if(!glview) 
	{
		//glview = GLViewImpl::createWithRect("THE", Rect(0, 0, 960, 640));
		//glview = GLViewImpl::createWithRect("THE", Rect(0, 0, 1280, 720));
		glview = GLViewImpl::createWithRect("THE", Rect(0, 0, 1440, 900));
		//glview = GLViewImpl::createWithRect("THE", Rect(0, 0, 1920, 1080));
		//glview = GLViewImpl::createWithFullScreen("THE");
		director->setOpenGLView(glview);
    }
	director->getOpenGLView()->setDesignResolutionSize(1440, 900, ResolutionPolicy::SHOW_ALL);
	//调整窗口位置
	RECT rect;
	LPRECT lpRect = &rect;
	GetWindowRect(glview->getWin32Window(), lpRect);
	float deltaX, deltaY;
	deltaX = lpRect->left - 100;
	deltaY = lpRect->top - 100;
	lpRect->top = 100;
	lpRect->bottom -= deltaY;
	lpRect->left = 100;
	lpRect->right -= deltaX;
	SetWindowPos(glview->getWin32Window(), HWND_TOP, lpRect->left, lpRect->top, lpRect->right - lpRect->left, lpRect->bottom - lpRect->top, SWP_SHOWWINDOW);
	//director->getOpenGLView()->setDesignResolutionSize(1280, 720, ResolutionPolicy::SHOW_ALL);
	// turn on display FPS
    director->setDisplayStats(true);

    // set FPS. the default value is 1.0/60 if you don't call this
    director->setAnimationInterval(1.0 / 60);

    FileUtils::getInstance()->addSearchPath("res");

    // create a scene. it's an autorelease object
    // run
    director->runWithScene(CSceneMenu::getInstance());

    return true;
}

// This function will be called when the app is inactive. When comes a phone call,it's be invoked too
void AppDelegate::applicationDidEnterBackground() {
    Director::getInstance()->stopAnimation();

    // if you use SimpleAudioEngine, it must be pause
    // SimpleAudioEngine::getInstance()->pauseBackgroundMusic();
}

// this function will be called when the app is active again
void AppDelegate::applicationWillEnterForeground() {
    Director::getInstance()->startAnimation();

    // if you use SimpleAudioEngine, it must resume here
    // SimpleAudioEngine::getInstance()->resumeBackgroundMusic();
}
