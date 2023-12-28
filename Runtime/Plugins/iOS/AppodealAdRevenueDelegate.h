#import <Foundation/Foundation.h>
#import <Appodeal/Appodeal.h>

typedef void (*AppodealAdRevenueCallback) ();

@interface AppodealAdRevenueDelegate : NSObject <AppodealAdRevenueDelegate>

@property (assign, nonatomic) AppodealAdRevenueCallback adRevenueReceivedCallback;

@end
