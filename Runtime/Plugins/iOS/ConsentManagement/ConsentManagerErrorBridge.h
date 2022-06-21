#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@class ConsentManagerErrorBridge;

FOUNDATION_EXPORT ConsentManagerErrorBridge *GetConsentManagerException(void);
FOUNDATION_EXPORT const char *CmeGetReason(void);
FOUNDATION_EXPORT int CmeGetCode(void);

@interface ConsentManagerErrorBridge : NSObject

- (instancetype)initWithCode:(NSInteger)code reason:(NSString *)reason;

@end

NS_ASSUME_NONNULL_END
