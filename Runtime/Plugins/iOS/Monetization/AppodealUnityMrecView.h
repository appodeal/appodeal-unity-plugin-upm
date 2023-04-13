#import <Appodeal/Appodeal.h>
#import <AppodealExtensions/AppodealExtensions-Swift.h>

@interface AppodealUnityMrecView : NSObject

+ (instancetype)sharedInstance;
UIViewController* RootViewControllerUnityMrec(void);
- (id)init;
- (void)setSharedMrecFrame:(CGFloat)XAxis YAxis:(CGFloat)YAxis;
- (void)hideMrecView;
- (void)showMrecView:(UIViewController*)rootViewController XAxis:(CGFloat)XAxis YAxis:(CGFloat)YAxis placement:(NSString*)placement;

@property(nonatomic, strong) AppodealUnifiedBannerView *mrecView;
@property (nonatomic, assign) BOOL onScreen;

@end
