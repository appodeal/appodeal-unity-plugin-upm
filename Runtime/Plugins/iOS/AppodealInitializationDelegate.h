#import <Foundation/Foundation.h>
#import <Appodeal/Appodeal.h>

typedef void (*AppodealInitializationCallback) ();

@interface AppodealInitializationDelegate : NSObject <AppodealInitializationDelegate>

@property (assign, nonatomic) AppodealInitializationCallback initializationCompletedCallback;

@end
