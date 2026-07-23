#import <Appodeal/Appodeal.h>

@interface AppodealUnityMrecView : NSObject

+ (instancetype)sharedInstance;
UIViewController* RootViewControllerUnityMrec(void);
- (id)init;
- (void)setSharedMrecFrame:(CGFloat)XAxis YAxis:(CGFloat)YAxis;
- (void)hideMrecView;
- (void)showMrecView:(UIViewController*)rootViewController XAxis:(CGFloat)XAxis YAxis:(CGFloat)YAxis placement:(NSString*)placement;
- (void)loadMrecView;
- (BOOL)isMrecViewReady;
- (BOOL)canShowMrecViewForPlacement:(NSString*)placement;
- (BOOL)isMrecViewPrecacheForPlacement:(NSString*)placement;
- (double)mrecViewPredictedEcpm;
- (void)setMrecViewAutoCache:(BOOL)autoCache;
- (BOOL)isMrecViewAutoCacheEnabled;

@property(nonatomic, strong) APDMRECView *mrecView;
@property (nonatomic, assign) BOOL onScreen;

@end
