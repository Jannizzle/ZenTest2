
#import "ZendeskSDK.h"

@interface ZendeskJSON: NSObject

+(NSString *)ZDKDispatcherResponseToJSON:(ZDKDispatcherResponse *) dispatcherResponse;
+(NSString *)NSArrayOfZDKCommentsToJSON:(NSArray *)items;
+(NSString *)NSArrayOfZDKHelpCenterArticlesToJSON:(NSArray *)items;
+(NSString *)NSArrayOfZDKHelpCenterCategoriesToJSON:(NSArray *)items;
+(NSString *)NSArrayOfZDKHelpCenterSectionsToJSON:(NSArray *)items;
+(NSString *)NSArrayOfZDKRequestsToJSON:(NSArray *)items;
+(NSString *)NSErrorToJSON:(NSError *) error;
+(NSString *)NSHTTPURLResponseToJSON:(NSHTTPURLResponse *) response;
+(NSString *)ZDKAttachmentToJSON:(ZDKAttachment *) attachment;
+(NSString *)ZDKCommentToJSON:(ZDKComment *) comment;
+(NSString *)ZDKSettingsToJSON:(ZDKSettings *) settings;
+(NSString *)ZDKUploadResponseToJSON:(ZDKUploadResponse *) uploadResponse;
+(NSString *)serializeJSONObject:(NSObject *) jsonObject;

@end
