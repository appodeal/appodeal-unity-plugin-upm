#if defined(__has_include) && __has_include("UnityAppController.h")
#import "UnityAppController.h"
#else
#import "EmptyUnityAppController.h"
#endif

#import <Appodeal/Appodeal.h>

#import "AppodealUnityMrecView.h"
#import "AppodealUnityBannerView.h"

#import "AppodealBannerDelegate.h"
#import "AppodealMrecViewDelegate.h"
#import "AppodealBannerViewDelegate.h"
#import "AppodealInterstitialDelegate.h"
#import "AppodealRewardedVideoDelegate.h"

#import "AppodealAdRevenueDelegate.h"
#import "AppodealIAPValidationDelegate.h"
#import "AppodealInitializationDelegate.h"

static AppodealUnityMrecView *mrecUnity;
static AppodealUnityBannerView *bannerUnity;

UIViewController *RootViewController() {
    return ((UnityAppController *)[UIApplication sharedApplication].delegate).rootViewController;
}

static NSDateFormatter *DateFormatter() {
    static dispatch_once_t onceToken;
    static NSDateFormatter *formatter;
    dispatch_once(&onceToken, ^{
        formatter = [[NSDateFormatter alloc] init];
        formatter.dateFormat = @"dd/MM/yyyy";
    });
    return formatter;
}

static NSString *NSStringFromUTF8String(const char *bytes) {
    return bytes ? @(bytes) : nil;
}

static NSDictionary <NSString *, id> *NSDictionaryFromUTF8String(const char *cString) {
    NSString *string = [NSString stringWithUTF8String:cString];
    if ([string length] == 0) return nil;
    NSArray *pairs = [string componentsSeparatedByString:@","];
    NSMutableDictionary <NSString *, id> *outputDict = [NSMutableDictionary dictionaryWithCapacity:pairs.count];
    [pairs enumerateObjectsUsingBlock:^(NSString *pair, NSUInteger idx, BOOL *stop) {
        NSArray <NSString *> *splited = [pair componentsSeparatedByString:@"="];
        NSString *key = splited.firstObject;
        NSString *value = splited.lastObject;
        NSArray *valueSplitted = [value componentsSeparatedByString:@":"];
        if (key) {
            if ([valueSplitted.firstObject isEqualToString:@"System.Int32"]) {
                outputDict[key] = @([valueSplitted.lastObject intValue]);
            }
            else if ([valueSplitted.firstObject isEqualToString:@"System.Double"]) {
                outputDict[key] = @([valueSplitted.lastObject doubleValue]);
            }
            else if ([valueSplitted.firstObject isEqualToString:@"System.Boolean"]) {
                outputDict[key] = @([valueSplitted.lastObject boolValue]);
            }
            else if ([valueSplitted.firstObject isEqualToString:@"System.String"]){
                outputDict[key] = valueSplitted.lastObject;
            }
        }
    }];
    return outputDict;
}

void AppodealInitialize(const char *apiKey, int types, const char *pluginVer, const char *engineVer) {
    [Appodeal setFramework:APDFrameworkUnity version: [NSString stringWithUTF8String:engineVer]];
    [Appodeal setPluginVersion:[NSString stringWithUTF8String:pluginVer]];
    [Appodeal initializeWithApiKey:[NSString stringWithUTF8String:apiKey] types:types];
}

BOOL AppodealIsInitialized(int types) {
    return [Appodeal isInitializedForAdType:types];
}

BOOL AppodealShowAd(int style) {
    return [Appodeal showAd:style rootViewController: RootViewController()];
}

BOOL AppodealShowAdForPlacement(int style, const char *placement) {
    return [Appodeal showAd:style forPlacement:[NSString stringWithUTF8String:placement] rootViewController:RootViewController()];
}

BOOL AppodealShowBannerAdViewForPlacement(int YAxis, int XAxis, const char *placement) {
    if (!bannerUnity) {
        bannerUnity = [AppodealUnityBannerView sharedInstance];
    }
    [bannerUnity showBannerView:RootViewController() XAxis:XAxis YAxis:YAxis placement:[NSString stringWithUTF8String:placement]];
    return false;
}

BOOL AppodealShowMrecAdViewForPlacement(int YAxis, int XAxis, const char *placement) {
    if (!mrecUnity) {
        mrecUnity = [AppodealUnityMrecView sharedInstance];
    }
    [mrecUnity showMrecView:RootViewController() XAxis:XAxis YAxis:YAxis placement:[NSString stringWithUTF8String:placement]];
    return false;
}

BOOL AppodealIsReadyWithStyle(int style) {
    return [Appodeal isReadyForShowWithStyle:style];
}

void AppodealCacheAd(int types) {
    [Appodeal cacheAd:types];
}

void AppodealSetAutoCache(BOOL autoCache, int types) {
    [Appodeal setAutocache:autoCache types:types];
}

void AppodealHideBanner() {
    [Appodeal hideBanner];
}

void AppodealHideBannerView() {
    if (bannerUnity) {
        [bannerUnity.bannerView removeFromSuperview];
    }
}

void AppodealHideMrecView() {
    if (mrecUnity) {
        [mrecUnity.mrecView removeFromSuperview];
    }
}

void AppodealSetSmartBanners(bool value) {
    [Appodeal setSmartBannersEnabled:value];
}

BOOL AppodealIsSmartBannersEnabled() {
    return [Appodeal isSmartBannersEnabled];
}

void AppodealSetTabletBanners(bool value) {
    if (!bannerUnity) {
        bannerUnity = [AppodealUnityBannerView sharedInstance];
    }

    if (value) {
        [Appodeal setPreferredBannerAdSize:kAppodealUnitSize_728x90];
    } else {
        [Appodeal setPreferredBannerAdSize:kAppodealUnitSize_320x50];
    }

    [bannerUnity setTabletBanner:value];
}

void AppodealSetBannerAnimation(BOOL value) {
    [Appodeal setBannerAnimationEnabled:value];
}

void AppodealSetBannerRotation(int leftBannerRotation, int rightBannerRotation) {
    [Appodeal setBannerLeftRotationAngleDegrees:leftBannerRotation rightRotationAngleDegrees: rightBannerRotation];
}

void AppodealSetLogLevel(int level) {
    switch (level) {
        case 1:
            [Appodeal setLogLevel:APDLogLevelOff];
            break;
        case 2:
            [Appodeal setLogLevel:APDLogLevelDebug];
            break;
        case 3:
            [Appodeal setLogLevel:APDLogLevelVerbose];
            break;
        default:
            break;
    }
}

void AppodealSetTestingEnabled(BOOL testingEnabled) {
    [Appodeal setTestingEnabled:testingEnabled];
}

void AppodealSetChildDirectedTreatment(BOOL value) {
    [Appodeal setChildDirectedTreatment:value];
}

char *AppodealGetNetworks(int types) {
    NSArray<NSString *> *networksArray = [Appodeal registeredNetworkNamesForAdType:types];
    NSString *networks = [[networksArray valueForKey:@"description"] componentsJoinedByString:@","];
    const char *output = [networks UTF8String];
    char *outputCopy = calloc([networks length]+1, 1);
    return strncpy(outputCopy, output, [networks length]);
}

void AppodealDisableNetwork(const char *networkName) {
    [Appodeal disableNetwork:[NSString stringWithUTF8String:networkName]];
}

void AppodealDisableNetworkForAdTypes(const char *networkName, int type) {
    [Appodeal disableNetworkForAdType:type name:[NSString stringWithUTF8String:networkName]];
}

void AppodealSetLocationTracking(BOOL value) {
    [Appodeal setLocationTracking:value];
}

void AppodealSetTriggerPrecacheCallbacks(int types, bool value) {
    [Appodeal setTriggerPrecacheCallbacks:value types:types];
}

char *AppodealGetVersion() {
    const char *cString = [[Appodeal getVersion] UTF8String];
    char *cStringCopy = calloc([[Appodeal getVersion] length]+1, 1);
    return strncpy(cStringCopy, cString, [[Appodeal getVersion] length]);
}

long AppodealGetSegmentId() {
    NSNumber *id = [Appodeal segmentId];
    return [id longValue];
}

char *AppodealGetRewardCurrency(const char *placement) {
    NSString *rewardCurrencyName = [[Appodeal rewardForPlacement:[NSString stringWithUTF8String:placement]] currencyName];
    const char *cString = [rewardCurrencyName UTF8String];
    char *cStringCopy = calloc([rewardCurrencyName length]+1, 1);
    return strncpy(cStringCopy, cString, [rewardCurrencyName length]);
}

double AppodealGetRewardAmount(const char *placement) {
    float rewardAmount = [[Appodeal rewardForPlacement:[NSString stringWithUTF8String:placement]] amount];
    return (double)rewardAmount;
}

double AppodealGetPredictedEcpm(int types) {
    return [Appodeal predictedEcpmForAdType:types];
}

double AppodealGetPredictedEcpmForPlacement(int adType, const char* placement) {
    return [Appodeal predictedEcpmForAdType:adType placement:[NSString stringWithUTF8String:placement]];
}

BOOL AppodealCanShow(int style) {
    return [Appodeal canShow:style forPlacement:@"default"];
}

BOOL AppodealCanShowWithPlacement(int style, const char *placement) {
    return [Appodeal canShow:style forPlacement:[NSString stringWithUTF8String:placement]];
}

BOOL AppodealIsPrecacheAd(int adType) {
    return [Appodeal isPrecacheAd:adType];
}

BOOL AppodealIsAutoCacheEnabled(int adType) {
    return [Appodeal isAutocacheEnabled:adType];
}

void AppodealSetCustomFilterBool(const char *name, BOOL value) {
    [Appodeal setCustomStateValue:[NSNumber numberWithBool:value] forKey:[NSString stringWithUTF8String:name]];
}

void AppodealSetCustomFilterInt(const char *name, int value) {
    [Appodeal setCustomStateValue:[NSNumber numberWithInt:value] forKey:[NSString stringWithUTF8String:name]];
}

void AppodealSetCustomFilterDouble(const char *name, double value) {
    [Appodeal setCustomStateValue:[NSNumber numberWithDouble:value] forKey:[NSString stringWithUTF8String:name]];
}

void AppodealSetCustomFilterString(const char *name, const char *value) {
    [Appodeal setCustomStateValue:[NSString stringWithUTF8String:value] forKey:[NSString stringWithUTF8String:name]];
}

void AppodealResetCustomFilter(const char *name) {
    [Appodeal setCustomStateValue:nil forKey:[NSString stringWithUTF8String:name]];
}

void AppodealSetExtraDataBool(const char *name, BOOL value) {
    [Appodeal setExtrasValue:[NSNumber numberWithBool:value] forKey:[NSString stringWithUTF8String:name]];
}

void AppodealSetExtraDataInt(const char *name, int value) {
    [Appodeal setExtrasValue:[NSNumber numberWithInt:value] forKey:[NSString stringWithUTF8String:name]];
}

void AppodealSetExtraDataDouble(const char *name, double value) {
    [Appodeal setExtrasValue:[NSNumber numberWithDouble:value] forKey:[NSString stringWithUTF8String:name]];
}

void AppodealSetExtraDataString(const char *name, const char *value) {
    [Appodeal setExtrasValue:[NSString stringWithUTF8String:value] forKey:[NSString stringWithUTF8String:name]];
}

void AppodealResetExtraData(const char *name) {
    [Appodeal setExtrasValue:nil forKey:[NSString stringWithUTF8String:name]];
}

void AppodealTrackInAppPurchase(int amount, const char *currency) {
    [[APDSdk sharedSdk] trackInAppPurchase:[NSNumber numberWithInt:amount] currency:[NSString stringWithUTF8String:currency]];
}

void AppodealSetUserId(const char *userid) {
    [Appodeal setUserId:[NSString stringWithUTF8String:userid]];
}

char *AppodealGetUserId() {
    const char *cString = [[Appodeal userId] UTF8String];
    char *cStringCopy = calloc([[Appodeal userId] length]+1, 1);
    return strncpy(cStringCopy, cString, [[Appodeal userId] length]);
}

void AppodealLogEvent(const char *eventName, const char *eventParams) {
    [Appodeal trackEvent:NSStringFromUTF8String(eventName) customParameters:NSDictionaryFromUTF8String(eventParams)];
}

void AppodealValidateInAppPurchase(const char *productIdentifier,
                                   const char *price,
                                   const char *currency,
                                   const char *transactionId,
                                   const char *additionalParams,
                                   int type,
                                   InAppPurchaseValidationSucceededCallback success,
                                   InAppPurchaseValidationFailedCallback failure) {
    NSString *productIdString = NSStringFromUTF8String(productIdentifier);
    NSString *priceString = NSStringFromUTF8String(price);
    NSString *currencyString = NSStringFromUTF8String(currency);
    NSString *transactionIdString = NSStringFromUTF8String(transactionId);
    NSDictionary *additionalParamsDict = NSDictionaryFromUTF8String(additionalParams);

    [Appodeal validateAndTrackInAppPurchase:productIdString
                                       type:(APDPurchaseType)type
                                      price:priceString
                                   currency:[currencyString substringWithRange:NSMakeRange(0, MIN(5,currencyString.length))]
                              transactionId:transactionIdString
                       additionalParameters:additionalParamsDict

                                    success:^(NSDictionary *data) {
        NSData *jsonData;
        NSError *jsonError;
        jsonData = [NSJSONSerialization dataWithJSONObject:data
                                                   options:0
                                                     error:&jsonError];
        if (jsonError) {
            failure ? failure("Invalid response") : nil;
        } else {
            NSString *JSONString = [[NSString alloc] initWithBytes:jsonData.bytes
                                                            length:jsonData.length
                                                          encoding:NSUTF8StringEncoding];
            success ? success(JSONString.UTF8String) : nil;
        }
    }
                                    failure:^(NSError *error) {
        NSString *errorString = (!error) ? @"unknown" : [NSString stringWithFormat:@"error: %@", error.localizedDescription];        
        failure ? failure(errorString.UTF8String) : nil;
    }];
}

static AppodealAdRevenueDelegate *AppodealAdRevenueDelegateInstance;
void AppodealSetAdRevenueDelegate(AppodealAdRevenueCallback adRevenueReceived) {

    AppodealAdRevenueDelegateInstance = [AppodealAdRevenueDelegate new];

    AppodealAdRevenueDelegateInstance.adRevenueReceivedCallback = adRevenueReceived;

    [Appodeal setAdRevenueDelegate:AppodealAdRevenueDelegateInstance];
}

static AppodealInitializationDelegate *AppodealInitializationDelegateInstance;
void AppodealSetInitializationDelegate(AppodealInitializationCallback initializationCompleted) {

    AppodealInitializationDelegateInstance = [AppodealInitializationDelegate new];

    AppodealInitializationDelegateInstance.initializationCompletedCallback = initializationCompleted;

    [Appodeal setInitializationDelegate:AppodealInitializationDelegateInstance];
}

static AppodealInterstitialDelegate *AppodealInterstitialDelegateInstance;
void AppodealSetInterstitialDelegate(AppodealInterstitialDidLoadCallback interstitialDidLoadAd,
                                     AppodealInterstitialCallbacks interstitialDidFailToLoadAd,
                                     AppodealInterstitialCallbacks interstitialDidFailToPresent,
                                     AppodealInterstitialCallbacks interstitialWillPresent,
                                     AppodealInterstitialCallbacks interstitialDidDismiss,
                                     AppodealInterstitialCallbacks interstitialDidClick,
                                     AppodealInterstitialCallbacks interstitialDidExpired) {

    AppodealInterstitialDelegateInstance = [AppodealInterstitialDelegate new];

    AppodealInterstitialDelegateInstance.interstitialDidLoadCallback = interstitialDidLoadAd;
    AppodealInterstitialDelegateInstance.interstitialDidFailToLoadAdCallback = interstitialDidFailToLoadAd;
    AppodealInterstitialDelegateInstance.interstitialDidFailToPresentCallback = interstitialDidFailToPresent;
    AppodealInterstitialDelegateInstance.interstitialWillPresentCallback = interstitialWillPresent;
    AppodealInterstitialDelegateInstance.interstitialDidDismissCallback = interstitialDidDismiss;
    AppodealInterstitialDelegateInstance.interstitialDidClickCallback = interstitialDidClick;
    AppodealInterstitialDelegateInstance.interstitialsDidExpiredCallback = interstitialDidExpired;

    [Appodeal setInterstitialDelegate:AppodealInterstitialDelegateInstance];
}

static AppodealBannerDelegate *AppodealBannerDelegateInstance;
void AppodealSetBannerDelegate(AppodealBannerDidLoadCallback bannerDidLoadAd,
                               AppodealBannerCallbacks bannerDidFailToLoadAd,
                               AppodealBannerCallbacks bannerDidClick,
                               AppodealBannerCallbacks bannerDidExpired,
                               AppodealBannerCallbacks bannerDidShow,
                               AppodealBannerCallbacks bannerDidFailToPresent) {

    AppodealBannerDelegateInstance = [AppodealBannerDelegate new];

    AppodealBannerDelegateInstance.bannerDidLoadAdCallback = bannerDidLoadAd;
    AppodealBannerDelegateInstance.bannerDidFailToLoadAdCallback = bannerDidFailToLoadAd;
    AppodealBannerDelegateInstance.bannerDidClickCallback = bannerDidClick;
    AppodealBannerDelegateInstance.bannerDidExpiredCallback = bannerDidExpired;
    AppodealBannerDelegateInstance.bannerDidShowCallback = bannerDidShow;
    AppodealBannerDelegateInstance.bannerDidFailToPresentCallback = bannerDidFailToPresent;

    [Appodeal setBannerDelegate:AppodealBannerDelegateInstance];
}

static AppodealBannerViewDelegate *AppodealBannerViewDelegateInstance;
void AppodealSetBannerViewDelegate(AppodealBannerViewDidLoadCallback bannerViewDidLoadAd,
                                   AppodealBannerViewCallbacks bannerViewDidFailToLoadAd,
                                   AppodealBannerViewCallbacks bannerViewDidClick,
                                   AppodealBannerViewCallbacks bannerViewDidShow,
                                   AppodealBannerViewCallbacks bannerViewDidFailToPresent,
                                   AppodealBannerViewCallbacks bannerViewDidExpired) {

    AppodealBannerViewDelegateInstance = [AppodealBannerViewDelegate new];

    AppodealBannerViewDelegateInstance.bannerViewDidLoadAdCallback = bannerViewDidLoadAd;
    AppodealBannerViewDelegateInstance.bannerViewDidFailToLoadAdCallback = bannerViewDidFailToLoadAd;
    AppodealBannerViewDelegateInstance.bannerViewDidClickCallback = bannerViewDidClick;
    AppodealBannerViewDelegateInstance.bannerViewDidShowCallback = bannerViewDidShow;
    AppodealBannerViewDelegateInstance.bannerViewDidFailToPresentCallback = bannerViewDidFailToPresent;
    AppodealBannerViewDelegateInstance.bannerViewDidExpiredCallback = bannerViewDidExpired;

    if(!bannerUnity) {
        bannerUnity = [AppodealUnityBannerView sharedInstance];
    }
    [bannerUnity.bannerView setDelegate:AppodealBannerViewDelegateInstance];
}

static AppodealMrecViewDelegate *AppodealMrecViewDelegateInstance;
void AppodealSetMrecViewDelegate(AppodealMrecViewDidLoadCallback mrecViewDidLoadAd,
                                 AppodealMrecViewCallbacks mrecViewDidFailToLoadAd,
                                 AppodealMrecViewCallbacks mrecViewDidClick,
                                 AppodealMrecViewCallbacks mrecViewDidShow,
                                 AppodealMrecViewCallbacks mrecViewDidFailToPresent,
                                 AppodealMrecViewCallbacks mrecViewDidExpired) {

    AppodealMrecViewDelegateInstance = [AppodealMrecViewDelegate new];

    AppodealMrecViewDelegateInstance.mrecViewDidLoadAdCallback = mrecViewDidLoadAd;
    AppodealMrecViewDelegateInstance.mrecViewDidFailToLoadAdCallback = mrecViewDidFailToLoadAd;
    AppodealMrecViewDelegateInstance.mrecViewDidClickCallback = mrecViewDidClick;
    AppodealMrecViewDelegateInstance.mrecViewDidShowCallback = mrecViewDidShow;
    AppodealMrecViewDelegateInstance.mrecViewDidFailToPresentCallback = mrecViewDidFailToPresent;
    AppodealMrecViewDelegateInstance.mrecViewDidExpiredCallback = mrecViewDidExpired;

    if (!mrecUnity) {
        mrecUnity = [AppodealUnityMrecView sharedInstance];
    }
    [mrecUnity.mrecView setDelegate:AppodealMrecViewDelegateInstance];
}

static AppodealRewardedVideoDelegate *AppodealRewardedVideoDelegateInstance;
void AppodealSetRewardedVideoDelegate(AppodealRewardedVideoDidLoadCallback rewardedVideoDidLoadAd,
                                      AppodealRewardedVideoCallbacks rewardedVideoDidFailToLoadAd,
                                      AppodealRewardedVideoCallbacks rewardedVideoDidFailToPresent,
                                      AppodealRewardedVideoDidDismissCallback rewardedVideoWillDismiss,
                                      AppodealRewardedVideoDidFinishCallback rewardedVideoDidFinish,
                                      AppodealRewardedVideoCallbacks rewardedVideoDidPresent,
                                      AppodealRewardedVideoCallbacks rewardedVideoDidExpired,
                                      AppodealRewardedVideoCallbacks rewardedVideoDidReceiveTap) {

    AppodealRewardedVideoDelegateInstance = [AppodealRewardedVideoDelegate new];

    AppodealRewardedVideoDelegateInstance.rewardedVideoDidLoadAdCallback = rewardedVideoDidLoadAd;
    AppodealRewardedVideoDelegateInstance.rewardedVideoDidFailToLoadAdCallback = rewardedVideoDidFailToLoadAd;
    AppodealRewardedVideoDelegateInstance.rewardedVideoDidFailToPresentCallback = rewardedVideoDidFailToPresent;
    AppodealRewardedVideoDelegateInstance.rewardedVideoWillDismissCallback = rewardedVideoWillDismiss;
    AppodealRewardedVideoDelegateInstance.rewardedVideoDidFinishCallback = rewardedVideoDidFinish;
    AppodealRewardedVideoDelegateInstance.rewardedVideoDidPresentCallback = rewardedVideoDidPresent;
    AppodealRewardedVideoDelegateInstance.rewardedVideoDidExpireCallback = rewardedVideoDidExpired;
    AppodealRewardedVideoDelegateInstance.rewardedVideoDidReceiveTapActionCallback = rewardedVideoDidReceiveTap;

    [Appodeal setRewardedVideoDelegate:AppodealRewardedVideoDelegateInstance];
}
