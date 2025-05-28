#import <Foundation/Foundation.h>
#import <Appodeal/Appodeal.h>

typedef void (*PurchaseValidationSucceededCallback)(const char* purchaseJson);
typedef void (*PurchaseValidationFailedCallback)(const char* reason);

@interface AppodealPurchaseDelegate : NSObject <AppodealPurchaseDelegate>

@property (assign, nonatomic) PurchaseValidationSucceededCallback purchaseValidationSucceededCallback;
@property (assign, nonatomic) PurchaseValidationFailedCallback purchaseValidationFailedCallback;

@end
