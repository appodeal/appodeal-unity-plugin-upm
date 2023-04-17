#import <Foundation/Foundation.h>
#import <Appodeal/Appodeal.h>
#import <AppodealExtensions/AppodealExtensions-Swift.h>

typedef void (*AppodealMrecViewCallbacks) ();
typedef void (*AppodealMrecViewDidLoadCallback) ();

@interface AppodealMrecViewDelegate : NSObject <AppodealUnifiedBannerViewDelegate>

@property (assign, nonatomic) AppodealMrecViewDidLoadCallback mrecViewDidLoadAdCallback;
@property (assign, nonatomic) AppodealMrecViewCallbacks mrecViewDidFailToLoadAdCallback;
@property (assign, nonatomic) AppodealMrecViewCallbacks mrecViewDidClickCallback;
@property (assign, nonatomic) AppodealMrecViewCallbacks mrecViewDidShowCallback;
@property (assign, nonatomic) AppodealMrecViewCallbacks mrecViewDidFailToPresentCallback;
@property (assign, nonatomic) AppodealMrecViewCallbacks mrecViewDidExpiredCallback;

@end
