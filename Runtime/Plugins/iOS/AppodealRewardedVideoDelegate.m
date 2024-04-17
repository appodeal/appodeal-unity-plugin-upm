#import "AppodealRewardedVideoDelegate.h"

@implementation AppodealRewardedVideoDelegate

-(void) rewardedVideoDidLoadAdIsPrecache:(BOOL)precache {
    if(self.rewardedVideoDidLoadAdCallback) {
        self.rewardedVideoDidLoadAdCallback(precache);
    }
}

-(void) rewardedVideoDidFailToLoadAd {
    if(self.rewardedVideoDidFailToLoadAdCallback) {
        self.rewardedVideoDidFailToLoadAdCallback();
    }
}

-(void) rewardedVideoDidFailToPresentWithError:(NSError *)error {
    if(self.rewardedVideoDidFailToPresentCallback) {
        self.rewardedVideoDidFailToPresentCallback();
    }
}

-(void) rewardedVideoWillDismissAndWasFullyWatched:(BOOL)wasFullyWatched {
    extern bool _didResignActive;
    if(_didResignActive) return;

    if(self.rewardedVideoWillDismissCallback) {
        self.rewardedVideoWillDismissCallback(wasFullyWatched);
    }
}

-(void) rewardedVideoDidPresent {
    if(self.rewardedVideoDidPresentCallback) {
        self.rewardedVideoDidPresentCallback();
    }
}

- (void)rewardedVideoDidFinish:(float)rewardAmount name:(NSString *)rewardName {
    extern bool _didResignActive;
    if(_didResignActive) return;

    if(self.rewardedVideoDidFinishCallback) {
        self.rewardedVideoDidFinishCallback((double)rewardAmount, [rewardName UTF8String]);
    }
}

- (void)rewardedVideoDidExpired {
    if(self.rewardedVideoDidExpireCallback) {
        self.rewardedVideoDidExpireCallback();
    }
}

- (void)rewardedVideoDidClick {
    if(self.rewardedVideoDidReceiveTapActionCallback) {
        self.rewardedVideoDidReceiveTapActionCallback();
    }
}

@end
