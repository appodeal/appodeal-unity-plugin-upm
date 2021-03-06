#import <Foundation/Foundation.h>
#import <Appodeal/Appodeal.h>

typedef void (*AppodealBannerCallbacks) ();
typedef void (*AppodealBannerDidLoadCallback) (int height, BOOL isPrecache);

@interface AppodealBannerDelegate : NSObject <AppodealBannerDelegate>

@property (assign, nonatomic) AppodealBannerDidLoadCallback bannerDidLoadAdCallback;
@property (assign, nonatomic) AppodealBannerCallbacks bannerDidFailToLoadAdCallback;
@property (assign, nonatomic) AppodealBannerCallbacks bannerDidClickCallback;
@property (assign, nonatomic) AppodealBannerCallbacks bannerDidExpiredCallback;
@property (assign, nonatomic) AppodealBannerCallbacks bannerDidShowCallback;
@property (assign, nonatomic) AppodealBannerCallbacks bannerDidFailToPresentCallback;

@end
