#import <Foundation/Foundation.h>
#import "ConsentManagerErrorBridge.h"
#import "ConsentBridge.h"

NS_ASSUME_NONNULL_BEGIN

@class ConsentForm;

typedef void (ConsentFormCallback)(void);
typedef void (ConsentFormCallbackError)(ConsentManagerErrorBridge *error);
typedef void (ConsentFormCallbackClosed)(ConsentBridge *consent);

FOUNDATION_EXPORT ConsentForm *GetConsentForm(void);
FOUNDATION_EXPORT void CfbWithListener(ConsentFormCallback onConsentFormLoaded,
                                       ConsentFormCallbackError onConsentFormError,
                                       ConsentFormCallback onConsentFormOpened,
                                       ConsentFormCallbackClosed onConsentFormClosed);

FOUNDATION_EXPORT ConsentForm *GetConsentForm(void);
FOUNDATION_EXPORT void CfLoad(void);
FOUNDATION_EXPORT void CfShow(void);
FOUNDATION_EXPORT bool CfIsLoaded(void);
FOUNDATION_EXPORT bool CfIsShowing(void);


@interface ConsentForm : NSObject

@end

NS_ASSUME_NONNULL_END
