//
//  ZendeskBinding.m
//  ZEN
//

#import "ZendeskSDK.h"
#import "ZendeskStringParams.h"
#import "ZendeskModalNavigationController.h"
#import "UnityAppController.h"
#import "ZendeskJSON.h"

extern void UnitySendMessage(const char *className, const char *methodName, const char *param);

char* ZDKMakeStringCopy(const char* string) {
    if (string == NULL) return NULL;
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

static char ZDKBase64EncodingTable[64] = {
    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
    'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
    'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
    'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'
};

NSString * ZDKBase64StringFromData(NSData * data, int length) {
    unsigned long ixtext, lentext;
    long ctremaining;
    unsigned char input[3], output[4];
    short i, charsonline = 0, ctcopy;
    const unsigned char *raw;
    NSMutableString *result;
    
    lentext = [data length];
    if (lentext < 1)
        return @"";
    result = [NSMutableString stringWithCapacity: lentext];
    raw = [data bytes];
    ixtext = 0;
    
    while (true) {
        ctremaining = lentext - ixtext;
        if (ctremaining <= 0)
            break;
        for (i = 0; i < 3; i++) {
            unsigned long ix = ixtext + i;
            if (ix < lentext)
                input[i] = raw[ix];
            else
                input[i] = 0;
        }
        output[0] = (input[0] & 0xFC) >> 2;
        output[1] = ((input[0] & 0x03) << 4) | ((input[1] & 0xF0) >> 4);
        output[2] = ((input[1] & 0x0F) << 2) | ((input[2] & 0xC0) >> 6);
        output[3] = input[2] & 0x3F;
        ctcopy = 4;
        switch (ctremaining) {
            case 1:
                ctcopy = 2;
                break;
            case 2:
                ctcopy = 3;
                break;
        }
        
        for (i = 0; i < ctcopy; i++)
            [result appendString: [NSString stringWithFormat: @"%c", ZDKBase64EncodingTable[output[i]]]];
        
        for (i = ctcopy; i < 4; i++)
            [result appendString: @"="];
        
        ixtext += 3;
        charsonline += 4;
        
        if ((length > 0) && (charsonline >= length))
            charsonline = 0;
    }
    return result;
}

NSData * ZDKBase64DataFromString(NSString * string)
{
    unsigned long ixtext, lentext;
    unsigned char ch, inbuf[4], outbuf[3];
    short i, ixinbuf;
    Boolean flignore, flendtext = false;
    const unsigned char *tempcstring;
    NSMutableData *theData;

    if (string == nil) {
        return [NSData data];
    }

    ixtext = 0;
    tempcstring = (const unsigned char *)[string UTF8String];
    lentext = [string length];
    theData = [NSMutableData dataWithCapacity: lentext];
    ixinbuf = 0;

    while (true) {
        if (ixtext >= lentext) {
            break;
        }

        ch = tempcstring [ixtext++];

        flignore = false;

        if ((ch >= 'A') && (ch <= 'Z')) {
            ch = ch - 'A';
        }
        else if ((ch >= 'a') && (ch <= 'z')) {
            ch = ch - 'a' + 26;
        }
        else if ((ch >= '0') && (ch <= '9')) {
            ch = ch - '0' + 52;
        }
        else if (ch == '+') {
            ch = 62;
        }
        else if (ch == '=') {
            flendtext = true;
        }
        else if (ch == '/') {
            ch = 63;
        }
        else {
            flignore = true; 
        }

        if (!flignore) {
            short ctcharsinbuf = 3;
            Boolean flbreak = false;

            if (flendtext) {
                if (ixinbuf == 0) {
                    break;
                }

                if ((ixinbuf == 1) || (ixinbuf == 2)) {
                    ctcharsinbuf = 1;
                }
                else {
                    ctcharsinbuf = 2;
                }

                ixinbuf = 3;
                flbreak = true;
            }

            inbuf [ixinbuf++] = ch;

            if (ixinbuf == 4) {
                ixinbuf = 0;

                outbuf[0] = (inbuf[0] << 2) | ((inbuf[1] & 0x30) >> 4);
                outbuf[1] = ((inbuf[1] & 0x0F) << 4) | ((inbuf[2] & 0x3C) >> 2);
                outbuf[2] = ((inbuf[2] & 0x03) << 6) | (inbuf[3] & 0x3F);

                for (i = 0; i < ctcharsinbuf; i++) {
                    [theData appendBytes: &outbuf[i] length: 1];
                }
            }

            if (flbreak) {
                break;
            }
        }
    }

    return theData;
}

#pragma mark - ZDKAvatarProvider

void _zendeskAvatarProviderGetAvatar(char * gameObjectName, char * callbackId, char * avatarUrl) {
    ZDKAvatarProvider *provider = [ZDKAvatarProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider getAvatarForUrl:GetStringParam(avatarUrl)
                 withCallback:^(UIImage *avatar, NSError *error) {
                     NSString *imageString = nil;
                     if (avatar) {
                         NSData *imageData = UIImagePNGRepresentation(avatar);
                         imageString = ZDKBase64StringFromData(imageData, imageData.length);
                     }
                     NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                           avatar && imageString ? imageString : [NSNull null], @"result",
                                           error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                           callbackIdHolder, @"callbackId",
                                           nil];
                     NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                     NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                     UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                      "didAvatarProviderGetAvatar",
                                      json.UTF8String);
                 }];
}

#pragma mark - ZDKConfig

void _zendeskConfigInitialize(char *applicationId, char *zendeskUrl, char *oAuthClientId) {
    [[ZDKConfig instance] initializeWithAppId:GetStringParam(applicationId)
                                   zendeskUrl:GetStringParam(zendeskUrl)
                                  andClientId:GetStringParam(oAuthClientId)];
    
}

void _zendeskConfigAuthenticateAnonymousIdentity(char * name, char * email, char * externalId) {
    ZDKAnonymousIdentity *identity = [ZDKAnonymousIdentity new];
    identity.name = GetStringParam(name);
    identity.email = GetStringParam(email);
    identity.externalId = GetStringParam(externalId);
    [ZDKConfig instance].userIdentity = identity;
}

void _zendeskConfigAuthenticateJwtUserIdentity(char * jwtUserIdentityString) {
    ZDKJwtIdentity * jwtUserIdentity = [[ZDKJwtIdentity alloc] initWithJwtUserIdentifier:GetStringParam(jwtUserIdentityString)];
    [ZDKConfig instance].userIdentity = jwtUserIdentity;
}

void _zendeskConfigReload() {
    [[ZDKConfig instance] reload];
}

void _zendeskConfigSetUserLocale(char * local) {
    [ZDKConfig instance].userLocale = GetStringParam(local);
}

#pragma mark - ZDKDeviceInfo

char* _zendeskDeviceType() {
    return ZDKMakeStringCopy([ZDKDeviceInfo deviceType].UTF8String);
}

double _zendeskTotalDeviceMemory() {
    return [ZDKDeviceInfo totalDeviceMemory];
}

double _zendeskFreeDiskspace() {
    return [ZDKDeviceInfo freeDiskspace];
}

double _zendeskTotalDiskspace() {
    return [ZDKDeviceInfo totalDiskspace];
}

float _zendeskBatteryLevel() {
    return [ZDKDeviceInfo batteryLevel];
}

char* _zendeskRegion() {
    return ZDKMakeStringCopy([ZDKDeviceInfo region].UTF8String);
}

char* _zendeskLanguage() {
    return ZDKMakeStringCopy([ZDKDeviceInfo language].UTF8String);
}

char* _zendeskDeviceInfoString() {
    return ZDKMakeStringCopy([ZDKDeviceInfo deviceInfoString].UTF8String);
}

char* _zendeskDeviceInfoDictionary() {
    // ZDKDeviceInfo:deviceInfoDictionary calls may cause crash.
    NSDictionary *result = [ZDKDeviceInfo deviceInfoDictionary];
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:result options:0 error:NULL];
    NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    return ZDKMakeStringCopy(json.UTF8String);
}

#pragma mark - ZDKDispatcher

void _zendeskDispatcherSetDebugLogging(BOOL enabled) {
    [ZDKDispatcher setDebugLogging:enabled];
}

#pragma mark - ZDKHelpCenter

void _zendeskShowHelpCenterWithNavController() {
    ZendeskModalNavigationController *modalNavController = [[ZendeskModalNavigationController alloc] init];
    [ZDKHelpCenter showHelpCenterWithNavController:modalNavController];
    UIViewController *rootViewController = [modalNavController.viewControllers firstObject];
    rootViewController.navigationItem.leftBarButtonItem = [[UIBarButtonItem alloc] initWithTitle:@"Close"
                                                                                           style:UIBarButtonItemStyleBordered
                                                                                          target:modalNavController
                                                                                          action:@selector(close:)];
    [UnityGetGLViewController() presentViewController:modalNavController animated:YES completion:nil];
}

void _zendeskShowHelpCenterFilterByArticleLabels(char *charArray[], int length) {
    NSMutableArray * newStrings = @[].mutableCopy;
    for (int i = 0; i < length; i++) {
        [newStrings addObject:GetStringParam(charArray[i])];
    }
    ZendeskModalNavigationController *modalNavController = [[ZendeskModalNavigationController alloc] init];
    [ZDKHelpCenter showHelpCenterWithNavController:modalNavController filterByArticleLabels:[newStrings copy]];
    UIViewController *rootViewController = [modalNavController.viewControllers firstObject];
    rootViewController.navigationItem.leftBarButtonItem = [[UIBarButtonItem alloc] initWithTitle:@"Close"
                                                                                           style:UIBarButtonItemStyleBordered
                                                                                          target:modalNavController
                                                                                          action:@selector(close:)];
    [UnityGetGLViewController() presentViewController:modalNavController animated:YES completion:nil];
}

#pragma mark - ZDKHelpCenterProvider

void _zendeskHelpCenterProviderGetCategoriesWithCallback(char * gameObjectName, char * callbackId) {
    ZDKHelpCenterProvider *provider = [ZDKHelpCenterProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider getCategoriesWithCallback:^(NSArray *items, NSError *error) {
        NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                              items ? [ZendeskJSON NSArrayOfZDKHelpCenterCategoriesToJSON:items] : [NSNull null], @"result",
                              error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                              callbackIdHolder, @"callbackId",
                              nil];
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
        NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                         "didHelpCenterProviderGetCategories",
                         json.UTF8String);
    }];
}

void _zendeskHelpCenterProviderGetSectionsForCategoryId(char * gameObjectName, char * callbackId, char * categoryId) {
    ZDKHelpCenterProvider *provider = [ZDKHelpCenterProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider getSectionsForCategoryId:GetStringParam(categoryId)
                          withCallback:^(NSArray *items, NSError *error) {
                              NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                                    items ? [ZendeskJSON NSArrayOfZDKHelpCenterSectionsToJSON:items] : [NSNull null], @"result",
                                                    error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                                    callbackIdHolder, @"callbackId",
                                                    nil];
                              NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                              NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                              UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                               "didHelpCenterProviderGetSectionsForCategoryId",
                                               json.UTF8String);
                          }];
}

void _zendeskHelpCenterProviderGetArticlesForSectionId(char * gameObjectName, char * callbackId, char * sectionId) {
    ZDKHelpCenterProvider *provider = [ZDKHelpCenterProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider getArticlesForSectionId:GetStringParam(sectionId)
                         withCallback:^(NSArray *items, NSError *error) {
                             NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                                   items ? [ZendeskJSON NSArrayOfZDKHelpCenterArticlesToJSON:items]  : [NSNull null], @"result",
                                                   error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                                   callbackIdHolder, @"callbackId",
                                                   nil];
                             NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                             NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                             UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                              "didHelpCenterProviderGetArticlesForSectionId",
                                              json.UTF8String);
                          }];
}

void _zendeskHelpCenterProviderSearchForArticlesUsingQuery(char * gameObjectName, char * callbackId, char * query) {
    ZDKHelpCenterProvider *provider = [ZDKHelpCenterProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider searchForArticlesUsingQuery:GetStringParam(query)
                             withCallback:^(NSArray *items, NSError *error) {
                                 NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                                       items ? [ZendeskJSON NSArrayOfZDKHelpCenterArticlesToJSON:items]  : [NSNull null], @"result",
                                                       error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                                       callbackIdHolder, @"callbackId",
                                                       nil];
                                 NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                                 NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                                 UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                                  "didHelpCenterProviderSearchForArticlesUsingQuery",
                                                  json.UTF8String);
                         }];
}

void _zendeskHelpCenterProviderSearchForArticlesUsingQueryAndLabels(char * gameObjectName, char * callbackId, char * query, char * labelsArray[], int labelsLength) {
    NSMutableArray * labels = @[].mutableCopy;
    for (int i = 0; i < labelsLength; i++) {
        [labels addObject:GetStringParam(labelsArray[i])];
    }
    ZDKHelpCenterProvider *provider = [ZDKHelpCenterProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider searchForArticlesUsingQuery:GetStringParam(query)
                                andLabels:labels
                             withCallback:^(NSArray *items, NSError *error) {
                                 NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                                       items ? [ZendeskJSON NSArrayOfZDKHelpCenterArticlesToJSON:items]  : [NSNull null], @"result",
                                                       error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                                       callbackIdHolder, @"callbackId",
                                                       nil];
                                 NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                                 NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                                 UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                                  "didHelpCenterProviderSearchForArticlesUsingQueryAndLabels",
                                                  json.UTF8String);
                             }];
}

void _zendeskHelpCenterProviderSearchForArticlesUsingHelpCenterSearch(char * gameObjectName, 
                                                                      char * callbackId, 
                                                                      char * query, 
                                                                      char * labelNames[], 
                                                                      int labelNamesLength, 
                                                                      char * locale,
                                                                      char * sideLoads[],
                                                                      int sideLoadsLength,
                                                                      int categoryId,
                                                                      int sectionId,
                                                                      int page,
                                                                      int resultsPerPage) {
    ZDKHelpCenterProvider *provider = [ZDKHelpCenterProvider new];
    ZDKHelpCenterSearch *helpCenterSearch = [ZDKHelpCenterSearch new];
    if (query != nil) {
          helpCenterSearch.query = GetStringParam(query);
    }
    if (labelNamesLength > 0 && labelNames != nil) {
          NSMutableArray * labelNamesArray = @[].mutableCopy;
          for (int i = 0; i < labelNamesLength; i++) {
                [labelNamesArray addObject:GetStringParam(labelNames[i])];
          }
          helpCenterSearch.labelNames = labelNamesArray;
    }
    if (locale != nil) {
          helpCenterSearch.locale = GetStringParam(locale);
    }
    if (sideLoads > 0 && sideLoads != nil) {
          NSMutableArray * sideLoadsArray = @[].mutableCopy;
          for (int i = 0; i < sideLoadsLength; i++) {
                [sideLoadsArray addObject:GetStringParam(sideLoads[i])];
          }
          helpCenterSearch.sideLoads = sideLoadsArray;
    }
    if (categoryId > -1) {
          helpCenterSearch.categoryId = [NSNumber numberWithInt:categoryId];
    }
    if (sectionId > -1) {
          helpCenterSearch.sectionId = [NSNumber numberWithInt:sectionId];
    }
    if (page > -1) {
          helpCenterSearch.page = [NSNumber numberWithInt:page];
    }
    if (resultsPerPage > -1) {
          helpCenterSearch.resultsPerPage = [NSNumber numberWithInt:resultsPerPage];
    }

    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider searchArticles:helpCenterSearch
                withCallback:^(NSArray *items, NSError *error) {
                    NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                          items ? [ZendeskJSON NSArrayOfZDKHelpCenterArticlesToJSON:items]  : [NSNull null], @"result",
                                          error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                          callbackIdHolder, @"callbackId",
                                          nil];
                    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                    NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                    UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                     "didHelpCenterProviderSearchForArticlesUsingHelpCenterSearch",
                                     json.UTF8String);
                             }];
}

void _zendeskHelpCenterProviderGetAttachmentForArticleId(char * gameObjectName, char * callbackId, char * articleId) {
    ZDKHelpCenterProvider *provider = [ZDKHelpCenterProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider getAttachmentForArticleId:GetStringParam(articleId)
                           withCallback:^(NSArray *items, NSError *error) {
                               NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                                     items ? [ZendeskJSON NSArrayOfZDKHelpCenterArticlesToJSON:items] : [NSNull null], @"result",
                                                     error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                                     callbackIdHolder, @"callbackId",
                                                     nil];
                               NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                               NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                               UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                                "didHelpCenterProviderGetAttachmentForArticleId",
                                                json.UTF8String);
                             }];
}

void _zendeskHelpCenterProviderGetArticlesByLabels(char * gameObjectName, char * callbackId, char * labelsArray[], int labelsLength) {
    NSMutableArray * labels = @[].mutableCopy;
    for (int i = 0; i < labelsLength; i++) {
        [labels addObject:GetStringParam(labelsArray[i])];
    }
    ZDKHelpCenterProvider *provider = [ZDKHelpCenterProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider getArticlesByLabels:labels
                     withCallback:^(NSArray *items, NSError *error) {
                         NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                               items ? [ZendeskJSON NSArrayOfZDKHelpCenterArticlesToJSON:items] : [NSNull null], @"result",
                                               error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                               callbackIdHolder, @"callbackId",
                                               nil];
                         NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                         NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                         UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                          "didHelpCenterGetArticlesByLabels",
                                          json.UTF8String);
                           }];
}

#pragma mark - ZDKLogger

void _zendeskLogEnable(BOOL enabled) {
    [ZDKLogger enable:enabled];
}

#pragma mark - ZDKRequests

void _zendeskShowRequestCreationWithNavController() {
    [ZDKRequests showRequestCreationWithNavController:(UINavigationController*)UnityGetGLViewController()];
}

void _zendeskShowRequestCreationWithNavControllerWithConfig(char * tags[],
                                                            int tagsLength,
                                                            char * additionalRequestInfo) {
      NSString *newAdditionalRequestInfo = GetStringParam(additionalRequestInfo);
      NSMutableArray * newTags = @[].mutableCopy;
      for (int i = 0; i < tagsLength; i++) {
            [newTags addObject:GetStringParam(tags[i])];
      }

      [ZDKRequests configure:^(ZDKAccount *account, ZDKRequestCreationConfig *config) {
            config.tags = newTags;
            config.additionalRequestInfo = newAdditionalRequestInfo;
      }];

      [ZDKRequests showRequestCreationWithNavController:(UINavigationController*)UnityGetGLViewController()];
}

void _zendeskShowRequestListWithNavController() {
    ZendeskModalNavigationController *modalNavController = [[ZendeskModalNavigationController alloc] init];
    [ZDKRequests showRequestListWithNavController:modalNavController];
    UIViewController *rootViewController = [modalNavController.viewControllers firstObject];
    rootViewController.navigationItem.leftBarButtonItem = [[UIBarButtonItem alloc] initWithTitle:@"Close"
                                                                                           style:UIBarButtonItemStyleBordered
                                                                                          target:modalNavController
                                                                                          action:@selector(close:)];
    
    [UnityGetGLViewController() presentViewController:modalNavController animated:YES completion:nil];
}

#pragma mark - ZDKRequestProvider

void _zendeskRequestProviderCreateRequest(char * gameObjectName, char * callbackId, char * subject, char * description, char *tagsArray[], int tagsLength) {
    NSMutableArray * tags = @[].mutableCopy;
    for (int i = 0; i < tagsLength; i++) {
        [tags addObject:GetStringParam(tagsArray[i])];
    }
    ZDKRequestProvider *provider = [ZDKRequestProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider createRequestWithSubject:GetStringParam(subject)
                        andDescription:GetStringParam(description)
                               andTags:tags
                           andCallback:^(id result, NSError *error) {
                               NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                                     result ? [ZendeskJSON ZDKDispatcherResponseToJSON:(ZDKDispatcherResponse *) result] : [NSNull null], @"result",
                                                     error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                                     callbackIdHolder, @"callbackId",
                                                     nil];
                               NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                               NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                               UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                                "didRequestProviderCreateRequest",
                                                json.UTF8String);
                           }];
}

void _zendeskRequestProviderCreateRequestWithAttachments(char * gameObjectName, char * callbackId, char * subject, char * description, char *tagsArray[], int tagsLength, char *attachmentsArray[], int attachmentsLength) {
    NSMutableArray * tags = @[].mutableCopy;
    for (int i = 0; i < tagsLength; i++) {
        [tags addObject:GetStringParam(tagsArray[i])];
    }
    
    NSMutableArray * attachments = @[].mutableCopy;
    for (int i = 0; i < attachmentsLength; i++) {
        ZDKUploadResponse *uploadResponse = [[ZDKUploadResponse alloc] init];
        uploadResponse.uploadToken = GetStringParam(attachmentsArray[i]);
        [attachments addObject:uploadResponse];
    }


    ZDKRequestProvider *provider = [ZDKRequestProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider createRequestWithSubject:GetStringParam(subject)
                           description:GetStringParam(description)
                                  tags:tags
                           attachments:attachments
                           andCallback:^(id result, NSError *error) {
                               NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                                     result ? [ZendeskJSON ZDKDispatcherResponseToJSON:(ZDKDispatcherResponse *) result] : [NSNull null], @"result",
                                                     error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                                     callbackIdHolder, @"callbackId",
                                                     nil];
                               NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                               NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                               UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                                "didRequestProviderCreateRequestWithAttachments",
                                                json.UTF8String);
                           }];
}

void _zendeskRequestProviderGetAllRequests(char * gameObjectName, char * callbackId) {
    ZDKRequestProvider *provider = [ZDKRequestProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider getAllRequestsWithCallback:^(NSArray *items, NSError *error) {
        NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                              items ? [ZendeskJSON NSArrayOfZDKRequestsToJSON:items] : [NSNull null], @"result",
                              error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                              callbackIdHolder, @"callbackId",
                              nil];
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
        NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                         "didRequestProviderGetAllRequests",
                         json.UTF8String);
    }];
}

void _zendeskRequestProviderGetAllRequestsByStatus(char * gameObjectName, char * callbackId, char * status) {
    ZDKRequestProvider *provider = [ZDKRequestProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider getRequestsByStatus:GetStringParam(status)
                     withCallback:^(NSArray *items, NSError *error) {
                         NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                               items ? [ZendeskJSON NSArrayOfZDKRequestsToJSON:items] : [NSNull null], @"result",
                                               error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                               callbackIdHolder, @"callbackId",
                                               nil];
                         NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                         NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                         UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                          "didRequestProviderGetAllRequestsByStatus",
                                          json.UTF8String);
                        }];
}

void _zendeskRequestProviderGetCommentsWithRequestId(char * gameObjectName, char * callbackId, char * requestId) {
    ZDKRequestProvider *provider = [ZDKRequestProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider getCommentsWithRequestId:GetStringParam(requestId)
                        withCallback:^(NSArray *commentsWithUsers, NSError *error) {
                            NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                                  commentsWithUsers ? [ZendeskJSON NSArrayOfZDKCommentsToJSON:commentsWithUsers] : [NSNull null], @"result",
                                                  error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                                  callbackIdHolder, @"callbackId",
                                                  nil];
                            NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                            NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                            UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                             "didRequestProviderGetCommentsWithRequestId",
                                             json.UTF8String);
                        }];
}

void _zendeskRequestProviderAddComment(char * gameObjectName, char * callbackId, char * comment, char * requestId) {
    ZDKRequestProvider *provider = [ZDKRequestProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider addComment:GetStringParam(comment)
            forRequestId:GetStringParam(requestId)
                          withCallback:^(ZDKComment *comment, NSError *error) {
                              NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                                    comment ? [ZendeskJSON ZDKCommentToJSON:comment] : [NSNull null], @"result",
                                                    error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                                    callbackIdHolder, @"callbackId",
                                                    nil];
                              NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                              NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                              UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                               "didRequestProviderAddComment",
                                               json.UTF8String);
                          }];
}

void _zendeskRequestProviderAddCommentWithAttachments(char * gameObjectName, char * callbackId, char * comment, char * requestId, char *attachmentsArray[], int attachmentsLength) {
    NSMutableArray * attachments = @[].mutableCopy;
    for (int i = 0; i < attachmentsLength; i++) {
        ZDKUploadResponse *uploadResponse = [[ZDKUploadResponse alloc] init];
        uploadResponse.uploadToken = GetStringParam(attachmentsArray[i]);
        [attachments addObject:uploadResponse];
    }

    ZDKRequestProvider *provider = [ZDKRequestProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider addComment:GetStringParam(comment)
            forRequestId:GetStringParam(requestId)
             attachments:attachments
            withCallback:^(ZDKComment *comment, NSError *error) {
                NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                      comment ? [ZendeskJSON ZDKCommentToJSON:comment] : [NSNull null], @"result",
                                      error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                      callbackIdHolder, @"callbackId",
                                      nil];
                NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                 "didRequestProviderAddCommentWithAttachments",
                                 json.UTF8String);
            }];
}

#pragma mark - ZDKRMA

void _zendeskRMAShowInView() {
    [ZDKRMA showInView:UnityGetGLView()];
}

void _zendeskRMAShowAlwaysInView() {
    [ZDKRMA showAlwaysInView:UnityGetGLView()];
}

void _zendeskRMAShowInViewWithConfiguration(char * additionalTags[],
                                            int additionalTagsLength,
                                            char * additionalRequestInfo,
                                            int dialogActions[],
                                            int dialogActionsLength,
                                            char * successImageName,
                                            char * errorImageName) {
      NSString *newAdditionalRequestInfo = GetStringParam(additionalRequestInfo);
      NSString *newSuccessImageName = GetStringParam(successImageName);
      NSString *newErrorImageName = GetStringParam(errorImageName);

      NSMutableArray * newAdditionalTags = @[].mutableCopy;
      for (int i = 0; i < additionalTagsLength; i++) {
            [newAdditionalTags addObject:GetStringParam(additionalTags[i])];
      }
      NSMutableArray * newDialogActions = @[].mutableCopy;
      for (int i = 0; i < dialogActionsLength; i++) {
            [newDialogActions addObject:[NSNumber numberWithInt:dialogActions[i]]];
      }

      [ZDKRMA configure:^(ZDKAccount *account, ZDKRMAConfigObject *config) {
            config.additionalTags = newAdditionalTags;
            config.additionalRequestInfo = newAdditionalRequestInfo;
            config.dialogActions = newDialogActions;
            config.successImageName = newSuccessImageName;
            config.errorImageName = newErrorImageName;
      }];

      [ZDKRMA showInView:UnityGetGLView()];
}

void _zendeskRMALogVisit() {
    [ZDKRMA logVisit];
}

#pragma mark - ZDKSettingsProvider

void _zendeskSettingsProviderGetSettings(char * gameObjectName, char * callbackId) {
    ZDKSettingsProvider *provider = [ZDKSettingsProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider getSdkSettingsWithCallback:^(ZDKSettings *settings, NSError *error) {
        NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                              settings ? [ZendeskJSON ZDKSettingsToJSON:settings] : [NSNull null], @"result",
                              error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                              callbackIdHolder, @"callbackId",
                              nil];
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
        NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                         "didSettingsProviderGetSettings",
                         json.UTF8String);
    }];
}

void _zendeskSettingsProviderGetSettingsWithLocale(char * gameObjectName, char * callbackId, char * locale) {
    ZDKSettingsProvider *provider = [ZDKSettingsProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider getSdkSettingsWithLocale:GetStringParam(locale)
                           andCallback:^(ZDKSettings *settings, NSError *error) {
                               NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                                     settings ? [ZendeskJSON ZDKSettingsToJSON:settings] : [NSNull null], @"result",
                                                     error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                                     callbackIdHolder, @"callbackId",
                                                     nil];
                               NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                               NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                               UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                                "didSettingsProviderGetSettingsWithLocale",
                                                json.UTF8String);
                           }];
}

#pragma mark - ZDKStringUtil

char* _zendeskCsvStringFromArray(char *charArray[], int length) {
    NSMutableArray * newStrings = @[].mutableCopy;
    for (int i = 0; i < length; i++) {
        [newStrings addObject:GetStringParam(charArray[i])];
    }
    return ZDKMakeStringCopy([ZDKStringUtil csvStringFromArray:[newStrings copy]].UTF8String);
}

#pragma mark - ZDKUploadProvider

void _zendeskZDKUploadProviderUploadAttachment(char * gameObjectName, char * callbackId, char * attachment, char * filename, char * contentType) {
    ZDKUploadProvider *provider = [ZDKUploadProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    NSString * contentTypeHolder = GetStringParam(contentType);

    NSData * attachmentData;
    if ([contentTypeHolder rangeOfString:@"text" options:NSCaseInsensitiveSearch].location != NSNotFound ||
        [contentTypeHolder rangeOfString:@"txt" options:NSCaseInsensitiveSearch].location != NSNotFound) {
        attachmentData = [GetStringParam(attachment) dataUsingEncoding:NSUTF8StringEncoding];
    }
    else if ([contentTypeHolder rangeOfString:@"image" options:NSCaseInsensitiveSearch].location != NSNotFound ||
             [contentTypeHolder rangeOfString:@"img" options:NSCaseInsensitiveSearch].location != NSNotFound) {
        attachmentData = ZDKBase64DataFromString(GetStringParam(attachment));
    }
    else {
      NSLog(@"Warning: Upload type not recognized (%@), handling attachment as string", contentTypeHolder);
      attachmentData = [GetStringParam(attachment) dataUsingEncoding:NSUTF8StringEncoding];
    }

    [provider uploadAttachment:attachmentData
                  withFilename:GetStringParam(filename)
                andContentType:GetStringParam(contentType)
                      callback:^(ZDKUploadResponse *uploadResponse, NSError *error) {
                          NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                                uploadResponse ? [ZendeskJSON ZDKUploadResponseToJSON:uploadResponse]: [NSNull null], @"result",
                                                error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                                callbackIdHolder, @"callbackId",
                                                nil];
                          NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                          NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                          UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                           "didUploadProviderUploadAttachment",
                                           json.UTF8String);
    }];
}

void _zendeskZDKUploadProviderDeleteUpload(char * gameObjectName, char * callbackId, char * uploadToken) {
    ZDKUploadProvider *provider = [ZDKUploadProvider new];
    NSString * gameObjectNameHolder = GetStringParam(gameObjectName);
    NSString * callbackIdHolder = GetStringParam(callbackId);
    [provider deleteUpload:GetStringParam(uploadToken)
               andCallback:^(NSString *responseCode, NSError *error) {
                   NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
                                         responseCode ? responseCode : [NSNull null], @"result",
                                         error ? [ZendeskJSON NSErrorToJSON:error] : [NSNull null], @"error",
                                         callbackIdHolder, @"callbackId",
                                         nil];
                   NSData *jsonData = [NSJSONSerialization dataWithJSONObject:data options:0 error:NULL];
                   NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                   UnitySendMessage(ZDKMakeStringCopy(gameObjectNameHolder.UTF8String),
                                    "didUploadProviderDeleteUpload",
                                    json.UTF8String);
                      }];
}
