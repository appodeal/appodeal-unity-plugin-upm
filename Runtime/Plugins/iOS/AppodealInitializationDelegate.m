#import "AppodealInitializationDelegate.h"

@implementation AppodealInitializationDelegate

-(void) appodealSDKDidInitialize {
    if(self.initializationCompletedCallback) {
        self.initializationCompletedCallback();
    }
}

@end
