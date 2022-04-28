#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

FOUNDATION_EXPORT const char *ConsentGetZone(void);
FOUNDATION_EXPORT const char *ConsentGetStatus(void);
FOUNDATION_EXPORT const char *ConsentGetIabConsentString(void);
FOUNDATION_EXPORT const char *ConsentHasConsentForVendor(const char *bundle);
FOUNDATION_EXPORT const char *GetChar(NSString *message);
FOUNDATION_EXPORT const char *ConsentGetAuthorizationStatus(void);
@interface ConsentBridge : NSObject

@end

NS_ASSUME_NONNULL_END
