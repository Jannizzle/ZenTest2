

#import "ZendeskJSON.h"

@interface ZendeskJSON ()

@end

@implementation ZendeskJSON

+(NSString *)ZDKDispatcherResponseToJSON:(ZDKDispatcherResponse *) dispatcherResponse {
	NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:
		[ZendeskJSON NSHTTPURLResponseToJSON:dispatcherResponse.response], @"response",
		[[NSString alloc] initWithData:dispatcherResponse.data encoding:NSUTF8StringEncoding], @"data",
		nil];
    return [ZendeskJSON serializeJSONObject:dict];
}

+(NSString *)NSArrayOfZDKCommentsToJSON:(NSArray *)items {
	NSMutableArray *results = [[NSMutableArray alloc] init];
	for (ZDKComment *comment in items) {
		[results addObject:[ZendeskJSON ZDKCommentToJSON:comment]];
	}
	return [ZendeskJSON serializeJSONObject:[results copy]];
}

+(NSString *)NSArrayOfZDKHelpCenterArticlesToJSON:(NSArray *)items {
	NSMutableArray *results = [[NSMutableArray alloc] init];
	for (ZDKHelpCenterArticle *article in items) {
		NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
			article.sid, @"sid",
			article.section_id, @"section_id",
			article.title, @"title",
			article.body, @"body",
			article.author_name, @"author_name",
			article.author_id, @"author_id",
			article.article_details, @"article_details",
			article.articleParents, @"articleParents",
			[NSNumber numberWithDouble:article.created_at.timeIntervalSince1970], @"created_at",
			article.position, @"position",
			[NSNumber numberWithBool:article.outdated], @"outdated",
			nil];
		[results addObject:data];
	}
	return [ZendeskJSON serializeJSONObject:[results copy]];	
}

+(NSString *)NSArrayOfZDKHelpCenterCategoriesToJSON:(NSArray *)items {
	NSMutableArray *results = [[NSMutableArray alloc] init];
	for (ZDKHelpCenterCategory *category in items) {
		NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
			category.sid, @"sid",
			category.name, @"name",
			category.categoryDescription, @"categoryDescription",
			category.position, @"position",
			[NSNumber numberWithBool:category.outdated], @"outdated",
			nil];
		[results addObject:data];
	}
	return [ZendeskJSON serializeJSONObject:[results copy]];
}

+(NSString *)NSArrayOfZDKHelpCenterSectionsToJSON:(NSArray *)items {
	NSMutableArray *results = [[NSMutableArray alloc] init];
	for (ZDKHelpCenterSection *section in items) {
		NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
			section.sid, @"sid",
			section.category_id, @"category_id",
			section.name, @"name",
			section.sectionDescription, @"sectionDescription",
			section.position, @"position",
			[NSNumber numberWithBool:section.outdated], @"outdated",
			nil];
		[results addObject:data];
	}
	return [ZendeskJSON serializeJSONObject:[results copy]];
}

+(NSString *)NSArrayOfZDKRequestsToJSON:(NSArray *)items {
	NSMutableArray *results = [[NSMutableArray alloc] init];
	for (ZDKRequest *request in items) {
		NSDictionary *data = [NSDictionary dictionaryWithObjectsAndKeys:
			request.requestId, @"requestId",
			request.requesterId, @"requesterId",
			request.status, @"status",
			request.subject, @"subject",
			request.requestDescription, @"requestDescription",
			[NSNumber numberWithDouble:request.createdAt.timeIntervalSince1970], @"createdAt",
			[NSNumber numberWithDouble:request.updateAt.timeIntervalSince1970], @"updateAt",
			[NSNumber numberWithDouble:request.publicUpdatedAt.timeIntervalSince1970], @"publicUpdatedAt",
			[NSNumber numberWithDouble:request.lastViewed.timeIntervalSince1970], @"lastViewed",
			nil];
		[results addObject:data];
	}
	return [ZendeskJSON serializeJSONObject:[results copy]];
}

+(NSString *)NSErrorToJSON:(NSError *) error {
	NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:
		[NSNumber numberWithInteger:error.code], @"code",
		error.domain, @"domain",
		//error.userInfo, @"userInfo", dict has sub-objects that need to be converted
		error.localizedDescription, @"localizedDescription",
		error.localizedFailureReason, @"localizedFailureReason",
		nil];
    return [ZendeskJSON serializeJSONObject:dict];
}

+(NSString *)NSHTTPURLResponseToJSON:(NSHTTPURLResponse *) response {
	NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:
		[NSNumber numberWithInteger:response.statusCode], @"statusCode",
		[NSHTTPURLResponse localizedStringForStatusCode:response.statusCode], @"localizedStringForStatusCode",
		nil];
    return [ZendeskJSON serializeJSONObject:dict];
}

+(NSString *)ZDKAttachmentToJSON:(ZDKAttachment *) attachment {
	//Thumbnails associated with the attachment. A thumbnail is an attachment with a nil thumbnails array.
	NSMutableArray *thumbnails = nil; 
	if (attachment.thumbnails) {
		thumbnails = [[NSMutableArray alloc] init];
		for (ZDKAttachment *thumbnail in attachment.thumbnails) {
			[thumbnails addObject:[ZendeskJSON ZDKAttachmentToJSON:thumbnail]];
		}
	}

	NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:
		attachment.attachmentId, @"attachmentId",
		attachment.filename, @"filename",
		attachment.contentURLString, @"contentURLString",
		attachment.contentType, @"contentType",
		attachment.size, @"size",
		thumbnails, @"thumbnails",
		nil];
    return [ZendeskJSON serializeJSONObject:dict];
}

+(NSString *)ZDKCommentToJSON:(ZDKComment *) comment {
	NSMutableArray *attachmentResults = [[NSMutableArray alloc] init];
	for (ZDKAttachment *attachment in comment.attachments) {
		[attachmentResults addObject:[ZendeskJSON ZDKAttachmentToJSON:attachment]];
	}

	NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:
		comment.commentId, @"commentId",
		comment.body, @"body",
		comment.authorId, @"authorId",
		[NSNumber numberWithDouble:comment.createdAt.timeIntervalSince1970], @"createdAt",
		attachmentResults, @"attachments",
		nil];

    return [ZendeskJSON serializeJSONObject:dict];
}

+(NSString *)ZDKSettingsToJSON:(ZDKSettings *) settings {
	NSDictionary *helpCenterSettings = [NSDictionary dictionaryWithObjectsAndKeys:
		[NSNumber numberWithBool:settings.appSettings.helpCenterSettings.enabled], @"enabled",
        settings.appSettings.helpCenterSettings.locale, @"locale",
        nil];
	NSDictionary *contactUsSettings = [NSDictionary dictionaryWithObjectsAndKeys:
        settings.appSettings.contactUsSettings.tags, @"tags",
        nil];
	NSDictionary *conversationsSettings = [NSDictionary dictionaryWithObjectsAndKeys:
        [NSNumber numberWithBool:settings.appSettings.conversationsSettings.enabled], @"enabled",
        nil];
	NSDictionary *rateMyAappSettings = [NSDictionary dictionaryWithObjectsAndKeys:
        [NSNumber numberWithBool:settings.appSettings.rateMyAappSettings.enabled], @"enabled",
        settings.appSettings.rateMyAappSettings.visits, @"visits",
        settings.appSettings.rateMyAappSettings.duration, @"duration",
        settings.appSettings.rateMyAappSettings.delay, @"delay",
        settings.appSettings.rateMyAappSettings.tags, @"tags",
        settings.appSettings.rateMyAappSettings.appStoreUrl, @"appStoreUrl",
        nil];

	NSDictionary *appSettings = [NSDictionary dictionaryWithObjectsAndKeys:
        settings.appSettings.authentication, @"authentication",
        helpCenterSettings, @"helpCenterSettings",
        contactUsSettings, @"contactUsSettings",
        conversationsSettings, @"conversationsSettings",
        rateMyAappSettings, @"rateMyAappSettings",
        nil];

	NSDictionary *attachmentSettings = [NSDictionary dictionaryWithObjectsAndKeys:
        [NSNumber numberWithBool:settings.accountSettings.attachmentSettings.enabled], @"enabled",
        settings.accountSettings.attachmentSettings.maxAttachmentSize, @"maxAttachmentSize",
        nil];
	NSDictionary *accountSettings = [NSDictionary dictionaryWithObjectsAndKeys:
        attachmentSettings, @"attachmentSettings",
        nil];

	NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:
        appSettings, @"appSettings",
        accountSettings, @"accountSettings",
        nil];
    return [ZendeskJSON serializeJSONObject:dict];
}

+(NSString *)ZDKUploadResponseToJSON:(ZDKUploadResponse *) uploadResponse {
	NSDictionary *dict = [NSDictionary dictionaryWithObjectsAndKeys:
		uploadResponse.uploadToken, @"uploadToken",
		[ZendeskJSON ZDKAttachmentToJSON:uploadResponse.attachment], @"attachment",
		nil];
	return [ZendeskJSON serializeJSONObject:dict];
}

+(NSString *)serializeJSONObject:(NSObject *) jsonObject {
	NSData *jsonData = [NSJSONSerialization dataWithJSONObject:jsonObject options:0 error:NULL];
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

@end
