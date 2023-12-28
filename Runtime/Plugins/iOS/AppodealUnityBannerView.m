#if defined(__has_include) && __has_include("UnityAppController.h")
#import "UnityAppController.h"
#else
#import "EmptyUnityAppController.h"
#endif

#import <Foundation/Foundation.h>
#import "AppodealUnityBannerView.h"

#define BANNER_X_POSITION_SMART     -1
#define BANNER_X_POSITION_CENTER    -2
#define BANNER_X_POSITION_RIGHT     -3
#define BANNER_X_POSITION_LEFT      -4
#define BANNER_Y_POSITION_BOTTOM    -1
#define BANNER_Y_POSITION_TOP       -2

@implementation AppodealUnityBannerView

+ (instancetype)sharedInstance {
    static AppodealUnityBannerView *sharedInstance;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[self alloc] init];
    });
    return sharedInstance;
}

UIViewController* RootViewControllerUnityBannerView() {
    return ((UnityAppController *)[UIApplication sharedApplication].delegate).rootViewController;
}

- (id)init {
    self = [super init];
    _tabletBanner = YES;
    [self reinitAppodealBannerView];
    return self;
}

- (void)setTabletBanner:(BOOL)value {
    _tabletBanner = value;
    [self reinitAppodealBannerView];
}

- (void)reinitAppodealBannerView {
    BOOL tabletOrPhoneSize = [[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPad && self.tabletBanner;
    CGSize size = tabletOrPhoneSize ? kAPDAdSize728x90 : kAPDAdSize320x50;
    self.bannerView = [[APDBannerView alloc] initWithSize:size];
    self.onScreen = NO;
}

- (void)setSharedBannerFrame:(CGFloat)XAxis YAxis:(CGFloat)YAxis {
    UIViewAutoresizing mask = UIViewAutoresizingNone;

    UIView *superView = RootViewControllerUnityBannerView().view;
    CGSize  superviewSize = RootViewControllerUnityBannerView().view.bounds.size;
    CGFloat screenScale = [[UIScreen mainScreen] scale];

    CGFloat bannerHeight    = self.bannerView.frame.size.height;
    CGFloat bannerWidth     = self.bannerView.frame.size.width;

    CGFloat xOffset = .0f;
    CGFloat yOffset = .0f;

    // Calculate X offset

    if (XAxis == BANNER_X_POSITION_SMART) {
        mask |= UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleBottomMargin | UIViewAutoresizingFlexibleTopMargin;
        self.bannerView.usesSmartSizing = YES;
        bannerWidth = superviewSize.width;
    } else if (XAxis == BANNER_X_POSITION_LEFT) {
        mask |= UIViewAutoresizingFlexibleRightMargin;
    } else if (XAxis == BANNER_X_POSITION_RIGHT) {
        mask |= UIViewAutoresizingFlexibleLeftMargin;
        xOffset = superviewSize.width - bannerWidth;
    } else if (XAxis == BANNER_X_POSITION_CENTER) {
        xOffset = (superviewSize.width - bannerWidth) / 2;
        mask |= UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleLeftMargin;
    } else if (XAxis / screenScale > superviewSize.width - bannerWidth) {
        NSLog(@"[Appodeal Banner view][error] Banner view x offset cannot be more than Screen width - actual banner width");
        xOffset = superviewSize.width - bannerWidth;
        mask |= UIViewAutoresizingFlexibleLeftMargin;
    } else if (XAxis < -4) {
        NSLog(@"[Appodeal Banner view][error] Banner view x offset cannot be less than 0");
        xOffset = 0;
    } else {
        mask |= UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleLeftMargin;
        xOffset = XAxis / screenScale;
    }

    // Calculate Y offset

    if (YAxis == BANNER_Y_POSITION_TOP) {
        mask |= UIViewAutoresizingFlexibleBottomMargin;
        if (@available(iOS 11.0, *)) {
            yOffset = superView.safeAreaInsets.top;
        }
    } else if (YAxis == BANNER_Y_POSITION_BOTTOM) {
        mask |= UIViewAutoresizingFlexibleTopMargin;
        if (@available(iOS 11.0, *)) {
            yOffset = superviewSize.height - bannerHeight - superView.safeAreaInsets.bottom;
        }
        else {
            yOffset = superviewSize.height - bannerHeight;
        }
    } else if (YAxis < -2) {
        NSLog(@"[Appodeal Banner view][error] Banner view y offset cannot be less than 0");
        yOffset = 0;
    } else if (YAxis / screenScale > superviewSize.height - bannerHeight) { // User defined offset more than banner width
        NSLog(@"[Appodeal Banner view][error] Banner view y offset cannot be more than Screen height - actual banner height");
        mask |= UIViewAutoresizingFlexibleTopMargin;
        yOffset = superviewSize.height - bannerHeight;
    } else if (YAxis == .0f) {
        mask |= UIViewAutoresizingFlexibleBottomMargin;
    } else {
        yOffset = YAxis / screenScale;
        mask |= UIViewAutoresizingFlexibleTopMargin | UIViewAutoresizingFlexibleBottomMargin;
    }

    NSLog(@"Creating banner frame with parameters: xOffset = %f, yOffset = %f", xOffset, yOffset);
    CGRect bannerRect = CGRectMake(xOffset, yOffset, bannerWidth, bannerHeight);
    [self.bannerView setAutoresizingMask:mask];
    [self.bannerView setFrame:bannerRect];
    [self.bannerView layoutSubviews];
}

- (void)showBannerView:(UIViewController*)rootViewController
                 XAxis:(CGFloat)XAxis
                 YAxis:(CGFloat)YAxis
             placement:(NSString*)placement {
    [self.bannerView removeFromSuperview];
    self.bannerView.rootViewController = rootViewController;
    self.bannerView.placement = placement;
    [rootViewController.view addSubview:self.bannerView];
    [rootViewController.view bringSubviewToFront:self.bannerView];
    [self setSharedBannerFrame:XAxis YAxis:YAxis];
    
    self.onScreen = YES;
    [self.bannerView loadAd];
}

- (void)hideBannerView {
    if(self.bannerView) {
        [self.bannerView removeFromSuperview];
        self.onScreen = NO;
    }
}

- (void)setupTouchProcessing {
    if (self.bannerView) {
        UnityDropViewTouchProcessing(self.bannerView);
        UnitySetViewTouchProcessing(self.bannerView, touchesTransformedToUnityViewCoords);
    }
}

@end
