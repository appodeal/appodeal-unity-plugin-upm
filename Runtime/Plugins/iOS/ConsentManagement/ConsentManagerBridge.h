#import <Foundation/Foundation.h>
#import "ConsentManagerErrorBridge.h"
#import "ConsentBridge.h"

NS_ASSUME_NONNULL_BEGIN

typedef void (ConsentInfoUpdatedCallback)(ConsentBridge *consent);
typedef void (ConsentInfoUpdatedFailedCallback)(ConsentManagerErrorBridge *error);

FOUNDATION_EXPORT void CmRequestConsentInfoUpdate(const char *appodealAppKey,
                                                  ConsentInfoUpdatedCallback onConsentInfoUpdated,
                                                  ConsentInfoUpdatedFailedCallback onFailedToUpdateConsentInfo);

FOUNDATION_EXPORT id GetConsentManager(void);
FOUNDATION_EXPORT id CmGetCustomVendor(const char *bundle);
FOUNDATION_EXPORT ConsentBridge *CmGetConsent(void);
FOUNDATION_EXPORT const char *GetConstChar(NSString *message);

FOUNDATION_EXPORT const char *CmGetStorage(void);
FOUNDATION_EXPORT const char *CmShouldShowConsentDialog(void);
FOUNDATION_EXPORT const char *CmGetConsentZone(void);
FOUNDATION_EXPORT const char *CmGetConsentStatus(void);
FOUNDATION_EXPORT const char *CmGetIabConsentString(void);

FOUNDATION_EXPORT void CmSetStorage(const char *storage);
FOUNDATION_EXPORT void CmSetCustomVendor(id customVendor);

FOUNDATION_EXPORT void SetCurrentError(NSError *error);
FOUNDATION_EXPORT ConsentManagerErrorBridge *GetCurrentErrorBridge(void);
FOUNDATION_EXPORT void CmDisableAppTrackingTransparencyRequest();

NS_ASSUME_NONNULL_END
