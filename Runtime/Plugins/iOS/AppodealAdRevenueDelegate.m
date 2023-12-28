#import "AppodealAdRevenueDelegate.h"

@implementation AppodealAdRevenueDelegate

- (void)didReceiveRevenueForAd:(id<AppodealAdRevenue>)ad {
    if(self.adRevenueReceivedCallback) {
        self.adRevenueReceivedCallback(
                                       [ad.adTypeString UTF8String],
                                       [ad.networkName UTF8String],
                                       [ad.adUnitName UTF8String],
                                       [ad.demandSource UTF8String],
                                       [ad.placement UTF8String],
                                       (double)ad.revenue,
                                       [ad.currency UTF8String],
                                       [ad.revenuePrecision UTF8String]);
    }
}

@end
