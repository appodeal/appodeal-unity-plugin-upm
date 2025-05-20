#import "AppodealPurchaseDelegate.h"

@implementation AppodealPurchaseDelegate

- (void)didReceivePurchase:(nullable NSDictionary<NSString*, id>*)successPurchases {
    if (!self.purchaseValidationSucceededCallback) return;

    if (!successPurchases) {
        self.purchaseValidationSucceededCallback(NULL);
        return;
    }

    NSError* error = nil;
    NSData* jsonData = [NSJSONSerialization dataWithJSONObject:successPurchases options:0 error:&error];

    if (!jsonData) {
        self.purchaseValidationSucceededCallback(NULL);
    } else {
        NSString* jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        self.purchaseValidationSucceededCallback([jsonString UTF8String]);
    }
}

- (void)didFailPurchase:(nullable NSError *)error {
    if (!self.purchaseValidationFailedCallback) return;

    if (!error) {
        self.purchaseValidationFailedCallback(NULL);
        return;
    }

    self.purchaseValidationFailedCallback([error.description UTF8String]);
}

@end
