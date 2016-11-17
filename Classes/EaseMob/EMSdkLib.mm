//
//  EMSdkLib.m
//  easemob
//
//  Created by lzj on 13/11/2016.
//  Copyright © 2016 EaseMob. All rights reserved.
//

#import "EMSdkLib.h"

@implementation EMSdkLib

static NSString* EM_U3D_OBJECT = @"emsdk_cb_object";


+ (instancetype) sharedSdkLib
{
    static EMSdkLib *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken,^{
        sharedInstance = [[EMSdkLib alloc] init];
        // Do any other initialisation stuff here
        
    });
    
    return sharedInstance;
}

- (void) initializeSDK:(NSString *)appKey
{
    EMOptions *options = [EMOptions optionsWithAppkey:appKey];
//    options.apnsCertName = @"istore_dev";
    [[EMClient sharedClient] initializeSDKWithOptions:options];
    
    [[EMClient sharedClient].chatManager addDelegate:self delegateQueue:nil];
}

- (void) applicationDidEnterBackground:(UIApplication *)application
{
    [[EMClient sharedClient] applicationDidEnterBackground:application];
}

- (void) applicationWillEnterForeground:(UIApplication *)application
{
    [[EMClient sharedClient] applicationWillEnterForeground:application];
}

- (int) createAccount:(NSString *)username withPwd:(NSString *)password
{
    EMError *error = [[EMClient sharedClient] registerWithUsername:username password:password];
    if(!error)
        return 0;
    else
        return [error code];
}

- (void) login:(NSString *)username withPwd:(NSString *)password
{
    [[EMClient sharedClient] loginWithUsername:username password:password completion:^(NSString *username, EMError *error){
        NSString *cbName = @"LoginCallback";
        if(!error){
            [self sendSuccessCallback:cbName];
        }else{
            [self sendErrorCallback:cbName withError:error];
        }
    }];
}

- (void) logout:(BOOL)flag
{
    [[EMClient sharedClient] logout:flag completion:^(EMError *error){
        NSString *cbName = @"LogoutCallback";
        if(!error)
        {
            [self sendSuccessCallback:cbName];
        }else{
            [self sendErrorCallback:cbName withError:error];
        }
    
    }];
}

- (void) sendTextMessage:(NSString *)content toUser:(NSString *)to callbackId:(int)callbackId chattype:(int)chattype
{
    EMTextMessageBody *body = [[EMTextMessageBody alloc] initWithText:content];
    NSString *from = [[EMClient sharedClient] currentUsername];
    
    EMMessage *message = [[EMMessage alloc] initWithConversationID:to from:from to:to body:body ext:nil];
    if(chattype == 0)
        message.chatType = EMChatTypeChat;// 设置为单聊消息
    else if (chattype == 1)
        message.chatType = EMChatTypeGroupChat;
    
    [self sendMessage:message CallbackId:callbackId];
}

- (void) sendFileMessage:(NSString *)path toUser:(NSString *)to callbackId:(int)callbackId chattype:(int)chattype
{
    EMFileMessageBody *body = [[EMFileMessageBody alloc] initWithLocalPath:path displayName:[path lastPathComponent]];
    NSString *from = [[EMClient sharedClient] currentUsername];
    
    EMMessage *message = [[EMMessage alloc] initWithConversationID:to from:from to:to body:body ext:nil];
    if(chattype == 0)
        message.chatType = EMChatTypeChat;// 设置为单聊消息
    else if (chattype == 1)
        message.chatType = EMChatTypeGroupChat;
    
    [self sendMessage:message CallbackId:callbackId];
}

- (NSString *) getAllContactsFromServer
{
    EMError *error = nil;
    NSArray *array = [[EMClient sharedClient].contactManager getContactsFromServerWithError:&error];
    if(!error && [array count] > 0)
        return [array componentsJoinedByString:@","];
    return @"user8";
}

//group API

- (void) createGroup:(NSString *)groupName desc:(NSString *)desc members:(NSString *)ms reason:(NSString *)reason maxUsers:(int)count type:(int)type callbackId:(int)cbId
{
    EMError *error = nil;
    NSString *cbName = @"CreateGroupCallback";
    EMGroupOptions *setting = [[EMGroupOptions alloc] init];
    setting.maxUsersCount = count;
    setting.style = (EMGroupStyle)type;
    NSArray *arr = [ms componentsSeparatedByString:@","];
    EMGroup *group = [[EMClient sharedClient].groupManager createGroupWithSubject:groupName description:desc invitees:arr message:reason setting:setting error:&error];
    if(!error)
    {
        NSString *json = [self toJson:[self group2dic:group]];
        if(json != nil)
            [self sendSuccessCallback:cbName CallbackId:cbId data:json];
    }else
    {
        [self sendErrorCallback:cbName withError:error];
    }
}

- (void) destroyGroup: (NSString *) groupId callbackId:(int) cbId
{
    EMError *error = nil;
    NSString *cbName = @"DestroyGroupCallback";

    [[EMClient sharedClient].groupManager destroyGroup: groupId error:&error];
    if (!error) {
        [self sendSuccessCallback:cbName CallbackId: cbId];
    } else {
        [self sendErrorCallback:cbName withError:error];
    }
}

- (void) updateGroupSubject:(NSString *) aSubject forGroup:(NSString *)aGroupId callbackId:(int)cbId
{
    NSString *cbName = @"UpdateGroupSubjectCallback";
    [[EMClient sharedClient].groupManager updateGroupSubject:aSubject forGroup:aGroupId completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];

}
- (void) updateGroupDescription:(NSString *) aDescription forGroup:(NSString *)aGroupId callbackId:(int)cbId
{
    NSString *cbName = @"UpdateGroupDescriptionCallback";
    [[EMClient sharedClient].groupManager updateDescription:aDescription forGroup:aGroupId completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}

- (NSString *)getJoinedGroups
{
    NSString *json = nil;
    NSArray *groupArray = [[EMClient sharedClient].groupManager getJoinedGroups];
    if ([groupArray count] > 0) {
        for (EMGroup *group in groupArray) {
            json = [json stringByAppendingString:[self toJson:[self group2dic:group]]];
        }
        return json;
    } else {
        return nil;
    }
}
- (NSString *)getGroupsWithoutPushNotification
{
    NSString *json = nil;
    EMError *error = nil;
    NSArray *groupArray = [[EMClient sharedClient].groupManager getGroupsWithoutPushNotification:&error];
    if (!error && [groupArray count] > 0) {
        for (EMGroup *group in groupArray) {
            json = [json stringByAppendingString:[self toJson:[self group2dic:group]]];
        }
        return json;
    } else {
        return nil;
    }
}

- (void)getJoinedGroupsFromServer:(int)cbId
{
    NSString *cbName = @"GetJoinedGroupFromServerCallback";

    [[EMClient sharedClient].groupManager getJoinedGroupsFromServerWithCompletion:^(NSArray *aList, EMError *aError) {
        if (!aError && [aList count] > 0) {
            NSString *json = nil;
            for (EMGroup *group in aList) {
                json = [json stringByAppendingString:[self toJson:[self group2dic:group]]];
            }
            [self sendSuccessCallback:cbName CallbackId: cbId data: json];
        } else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}

- (void)getGroupSpecificationFromServerById:(NSString *)aGroupId includeMembersList:(BOOL)aIncludeMemberList callbackId:(int)cbId
{
    NSString *cbName = @"GetGroupSpecificationFromServerByIdCallback";
    
    [[EMClient sharedClient].groupManager getGroupSpecificationFromServerByID:aGroupId includeMembersList:aIncludeMemberList completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError && aGroup) {
            NSString *json = nil;
            json = [json stringByAppendingString:[self toJson:[self group2dic:aGroup]]];
            [self sendSuccessCallback:cbName CallbackId: cbId data: json];
        } else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}

- (void)getGroupBlacklistFromServerById:(NSString *)aGroupId callbackId:(int)cbId
{
    NSString *cbName = @"GetGroupBlacklistFromServerByIdCallback";
    
    [[EMClient sharedClient].groupManager getGroupBlackListFromServerByID:aGroupId completion:^(NSArray *aList, EMError *aError) {
        if (!aError && [aList count] > 0) {
            NSString *json = nil;
            if(!aError && [aList count] > 0) {
                json = [aList componentsJoinedByString:@","];
            }
            [self sendSuccessCallback:cbName CallbackId: cbId data: json];
        } else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}

- (void) addMembers:(NSString *)ms toGroup: (NSString *) aGroupId withMessage:(NSString *)message callbackId:(int) cbId
{
    NSString *cbName = @"AddMembersCallback";
    NSArray *members = [ms componentsSeparatedByString:@","];
    [[EMClient sharedClient].groupManager addMembers:members toGroup:aGroupId message:message completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}
- (void) removeMembers:(NSString *)ms fromGroup: (NSString *) aGroupId callbackId:(int) cbId
{
    NSString *cbName = @"RemoveMembersCallback";
    NSArray *members = [ms componentsSeparatedByString:@","];
    [[EMClient sharedClient].groupManager removeMembers:members fromGroup:aGroupId completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}
- (void) blockMembers:(NSString *)ms fromGroup: (NSString *) aGroupId callbackId:(int) cbId
{
    NSString *cbName = @"BlockMembersCallback";
    NSArray *members = [ms componentsSeparatedByString:@","];
    [[EMClient sharedClient].groupManager blockMembers:members fromGroup:aGroupId completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}
- (void) unblockMembers:(NSString *)ms fromGroup: (NSString *) aGroupId callbackId:(int) cbId
{
    NSString *cbName = @"UnblockMembersCallback";
    NSArray *members = [ms componentsSeparatedByString:@","];
    [[EMClient sharedClient].groupManager unblockMembers:members fromGroup:aGroupId completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}

- (void) requestToJoinGroup:(NSString *)aGroupId withMessage:(NSString *)message callbackId:(int) cbId
{
    NSString *cbName = @"RequestToJoinGroupCallback";
    [[EMClient sharedClient].groupManager requestToJoinPublicGroup:aGroupId message:message completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}
- (void) leaveGroup:(NSString *)aGroupId callbackId:(int) cbId
{
    NSString *cbName = @"LeaveGroupCallback";
    [[EMClient sharedClient].groupManager leaveGroup:aGroupId completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}

- (void) approveJoinGroupRequest:(NSString *)aGroupId sender:(NSString *) aUsername callbackId:(int) cbId
{
    NSString *cbName = @"ApproveJoinGroupRequestCallback";
    [[EMClient sharedClient].groupManager approveJoinGroupRequest:aGroupId sender:aUsername completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}
- (void) declineJoinGroupRequest:(NSString *)aGroupId sender:(NSString *) aUsername reason:(NSString *)aReason callbackId:(int) cbId
{
    NSString *cbName = @"DeclineJoinGroupRequestCallback";
    [[EMClient sharedClient].groupManager declineJoinGroupRequest:aGroupId sender:aUsername reason:aReason completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}
- (void) acceptInvitationFromGroup:(NSString *)aGroupId inviter:(NSString *) aUsername callbackId:(int) cbId
{
    NSString *cbName = @"AcceptInvitationFromGroupCallback";
    [[EMClient sharedClient].groupManager acceptInvitationFromGroup:aGroupId inviter:aUsername completion:^(EMGroup *aGroup, EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}
- (void) declineInvitationFromGroup:(NSString *)aGroupId inviter:(NSString *) aUsername reason:(NSString *)aReason callbackId:(int) cbId
{
    NSString *cbName = @"DeclineInvitationFromGroupCallback";
    [[EMClient sharedClient].groupManager declineGroupInvitation:aGroupId inviter:aUsername reason:aReason completion:^(EMError *aError) {
        if (!aError) {
            [self sendSuccessCallback:cbName CallbackId: cbId];
        }
        else if (aError){
            [self sendErrorCallback:cbName withError:aError];
        }
    }];
}

//message delegates
- (void)messagesDidReceive:(NSArray *)aMessages;
{
    NSMutableArray *array = [NSMutableArray array];
    for(EMMessage *message in aMessages)
    {
        [array addObject:[self message2dic:message]];
    }
    
    NSString *json = [self toJson:array];
    if(json != nil)
    {
        [self sendCallback:@"MessageReceivedCallback" param:json];
    }
    
}

- (void) sendMessage:(EMMessage *)message CallbackId:(int)callbackId
{
    NSString *cbName = @"SendMessageCallback";
    [[EMClient sharedClient].chatManager sendMessage:message progress:^(int progress){
        [self sendInProgressCallback:cbName CallbackId:callbackId Progress:progress];
    } completion:^(EMMessage *message, EMError *error){
        if(error)
            [self sendErrorCallback:cbName CallbackId:callbackId withError:error];
        else
            [self sendSuccessCallback:cbName CallbackId:callbackId];
    }];
}

- (void) sendSuccessCallback:(NSString *)cbName
{
    [self sendCallback:EM_U3D_OBJECT cbName:cbName param:@"{\"on\":\"success\"}"];
}

- (void) sendSuccessCallback:(NSString *)cbName CallbackId:(int) callbackId
{
    NSMutableDictionary *dic = [NSMutableDictionary dictionary];
    [dic setObject:@"success" forKey:@"on"];
    [dic setObject:[NSNumber numberWithInt:callbackId] forKey:@"callbackid"];
    NSString *json = [self toJson:dic];
    if(json != nil)
        [self sendCallback:EM_U3D_OBJECT cbName:cbName param:json];
}

- (void) sendSuccessCallback:(NSString *)cbName CallbackId:(int) callbackId data:(NSString *)jsonData
{
    NSMutableDictionary *dic = [NSMutableDictionary dictionary];
    [dic setObject:@"success" forKey:@"on"];
    [dic setObject:[NSNumber numberWithInt:callbackId] forKey:@"callbackid"];
    [dic setObject:jsonData forKey:@"data"];
    NSString *json = [self toJson:dic];
    if(json != nil)
        [self sendCallback:EM_U3D_OBJECT cbName:cbName param:json];
}

- (void) sendErrorCallback:(NSString *)cbName withError:(EMError *)error
{
    NSMutableDictionary *dic = [NSMutableDictionary dictionary];
    [dic setObject:@"error" forKey:@"on"];
    [dic setObject:[NSNumber numberWithInt:[error code]]  forKey:@"code"];
    [dic setObject:[error errorDescription] forKey:@"message"];
    NSString *json = [self toJson:dic];
    if(json != nil){
        [self sendCallback:EM_U3D_OBJECT cbName:cbName param:json];
    }
}
- (void) sendErrorCallback:(NSString *)cbName CallbackId:(int) callbackId withError:(EMError *)error
{
    NSMutableDictionary *dic = [NSMutableDictionary dictionary];
    [dic setObject:@"error" forKey:@"on"];
    [dic setObject:[NSNumber numberWithInt:callbackId] forKey:@"callbackid"];
    [dic setObject:[NSNumber numberWithInt:[error code]]  forKey:@"code"];
    [dic setObject:[error errorDescription] forKey:@"message"];
    NSString *json = [self toJson:dic];
    if(json != nil){
        [self sendCallback:EM_U3D_OBJECT cbName:cbName param:json];
    }
}

- (void) sendInProgressCallback:(NSString *)cbName CallbackId:(int) callbackId Progress:(int)progress
{
    NSMutableDictionary *dic = [NSMutableDictionary dictionary];
    [dic setObject:@"progress" forKey:@"on"];
    [dic setObject:[NSNumber numberWithInt:callbackId] forKey:@"callbackid"];
    [dic setObject:[NSNumber numberWithInt:progress]  forKey:@"progress"];
    [dic setObject:@"status" forKey:@"status"];
    NSString *json = [self toJson:dic];
    if(json != nil){
        [self sendCallback:EM_U3D_OBJECT cbName:cbName param:json];
    }
}

- (void) sendCallback:(NSString *)objName cbName:(NSString *)cbName param:(NSString *)jsonParam
{
    NSLog(@"Send to objName=%@, cbName=%@, param=%@",objName,cbName,jsonParam);
    UnitySendMessage([objName UTF8String], [cbName UTF8String], [jsonParam UTF8String]);
}

- (void)sendCallback:(NSString *)cbName param:(NSString *)jsonParam
{
    [self sendCallback:EM_U3D_OBJECT cbName:cbName param:jsonParam];
}

- (NSString *)toJson:(id)ocData
{
    NSError *error;
    NSData *data = [NSJSONSerialization dataWithJSONObject:ocData options:0 error:&error];
    if(!error){
        return [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
    }
    return nil;
}

- (NSDictionary *) message2dic:(EMMessage *)message
{
    NSMutableDictionary *dic = [NSMutableDictionary dictionary];
    [dic setObject:message.messageId forKey:@"mMsgId"];
    [dic setObject:message.from forKey:@"mFrom"];
    [dic setObject:message.to forKey:@"mTo"];
    [dic setObject:message.isRead?@"true":@"false" forKey:@"mIsUnread"];
    [dic setObject:@"false" forKey:@"mIsListened"];//TODO
    [dic setObject:message.isReadAcked?@"true":@"false" forKey:@"mIsAcked"];
    [dic setObject:message.isDeliverAcked?@"true":@"false" forKey:@"mIsDelivered"];
    [dic setObject:[NSNumber numberWithLong:message.localTime] forKey:@"mLocalTime"];
    [dic setObject:[NSNumber numberWithLong:message.timestamp] forKey:@"mServerTime"];
    [dic setObject:[NSNumber numberWithInt:message.body.type] forKey:@"mType"];
    [dic setObject:[NSNumber numberWithInt:message.status] forKey:@"mStatus"];
    [dic setObject:[NSNumber numberWithInt:message.direction] forKey:@"mDirection"];
    [dic setObject:[NSNumber numberWithInt:message.chatType] forKey:@"mChatType"];
    
    if (message.body.type == EMMessageBodyTypeFile){
        EMFileMessageBody *body = (EMFileMessageBody *)message.body;
        [dic setObject:body.displayName forKey:@"mDisplayName"];
        [dic setObject:body.secretKey forKey:@"mSecretKey"];
        [dic setObject:body.localPath forKey:@"mLocalPath"];
        [dic setObject:body.remotePath forKey:@"mRemotePath"];
    }
    if(message.body.type == EMMessageBodyTypeText)
    {
        EMTextMessageBody *textBody = (EMTextMessageBody *)message.body;
        [dic setObject:textBody.text forKey:@"mTxt"];
    }
    
    return [NSDictionary dictionaryWithDictionary:dic];
}

- (NSDictionary *) group2dic:(EMGroup *)group
{
    NSMutableDictionary *dic = [NSMutableDictionary dictionary];
    [dic setObject:group.groupId forKey:@"mGroupId"];
    [dic setObject:group.subject forKey:@"mGroupName"];
    [dic setObject:group.isPublic?@"true":@"false" forKey:@"mIsPublic"];
    [dic setObject:@"true" forKey:@"mIsAllowInvites"];//TODO
    [dic setObject:group.isBlocked?@"true":@"false" forKey:@"mIsMsgBlocked"];
    [dic setObject:group.owner forKey:@"mOwner"];
    [dic setObject:[group.members componentsJoinedByString:@","] forKey:@"mMembers"];
    [dic setObject:group.description forKey:@"mDescription"];
    return [NSDictionary dictionaryWithDictionary:dic];
}

@end

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}
// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

extern "C" {
    int _createAccount(const char* username, const char*password)
    {
        return [[EMSdkLib sharedSdkLib] createAccount:CreateNSString(username) withPwd:CreateNSString(password)];
    }
    
    void _login(const char* username, const char* password)
    {
        [[EMSdkLib sharedSdkLib] login:CreateNSString(username) withPwd:CreateNSString(password)];
    }
    
    void _logout(bool flag)
    {
        [[EMSdkLib sharedSdkLib] logout:flag];
    }
    
    void _sendTextMessage(const char* content, const char* to, int callbackId,int chattype)
    {
        [[EMSdkLib sharedSdkLib] sendTextMessage:CreateNSString(content) toUser:CreateNSString(to) callbackId:callbackId chattype:chattype];
    }
    
    void _sendFileMessage(const char* path, const char* to, int callbackId,int chattype)
    {
        [[EMSdkLib sharedSdkLib] sendFileMessage:CreateNSString(path) toUser:CreateNSString(to) callbackId:callbackId chattype:chattype];
    }
    
    const char* _getAllContactsFromServer()
    {
        return MakeStringCopy([[[EMSdkLib sharedSdkLib] getAllContactsFromServer] UTF8String]);
    }
    
    void _createGroup (int callbackId, const char* groupName, const char* desc, const char* strMembers, const char* reason, int maxUsers, int style)
    {
        [[EMSdkLib sharedSdkLib] createGroup:CreateNSString(groupName) desc:CreateNSString(desc) members:CreateNSString(strMembers) reason:CreateNSString(reason) maxUsers:maxUsers type:style callbackId:callbackId];
    }

    void _destroyGroup(int callbackId, const char* groupId)
    {
        [[EMSdkLib sharedSdkLib] destroyGroup:CreateNSString(groupId) callbackId:callbackId];
    }

    void _updateGroupSubject(int callbackId, const char* subject, const char* groupId)
    {
        [[EMSdkLib sharedSdkLib] updateGroupSubject:CreateNSString(subject) forGroup:CreateNSString(groupId) callbackId:callbackId];
    }
 
    void _updateGroupDescription(int callbackId, const char* description, const char* groupId)
    {
        [[EMSdkLib sharedSdkLib] updateGroupSubject:CreateNSString(description) forGroup:CreateNSString(groupId) callbackId:callbackId];
    }

    const char* _getJoinedGroups()
    {
        return MakeStringCopy([[[EMSdkLib sharedSdkLib] getJoinedGroups] UTF8String]);
    }
    
    const char* _getGroupsWithoutPushNotification(int callbackId)
    {
        [[EMSdkLib sharedSdkLib] getGroupsWithoutPushNotification:callbackId];
    }
    
    void _getJoinedGroupsFromServer(int callbackId)
    {
        [[EMSdkLib sharedSdkLib] getJoinedGroupsFromServer:callbackId];
    }
    
    void _getGroupSpecificationFromServerById(int callbackId, const char* groupId, bool includeMemberList)
    {
        [[EMSdkLib sharedSdkLib] getGroupSpecificationFromServerById:CreateNSString(groupId) includeMembersList:includeMemberList callbackId:callbackId];
    }
    
    void _getGroupBlacklistFromServerById(int callbackId, const char* groupId)
    {
        [[EMSdkLib sharedSdkLib] getGroupBlacklistFromServerById:CreateNSString(groupId) callbackId:callbackId];
    }
    
    void _addMembers(int callbackId, const char* members, const char* toGroup, const char* message)
    {
        [[EMSdkLib sharedSdkLib] addMembers:CreateNSString(members) toGroup:CreateNSString(toGroup) withMessage:CreateNSString(message) callbackId:callbackId];
    }
    
    void _removeMembers(int callbackId, const char* members, const char* fromGroup, const char* message)
    {
        [[EMSdkLib sharedSdkLib] removeMembers:CreateNSString(members) fromGroup:CreateNSString(fromGroup) callbackId:callbackId];
    }
    
    void _blockMembers(int callbackId, const char* members, const char* toGroup, const char* message)
    {
        [[EMSdkLib sharedSdkLib] blockMembers:CreateNSString(members) fromGroup:CreateNSString(toGroup) callbackId:callbackId];
    }

    void _unblockMembers(int callbackId, const char* members, const char* fromGroup, const char* message)
    {
        [[EMSdkLib sharedSdkLib] unblockMembers:CreateNSString(members) fromGroup:CreateNSString(fromGroup) callbackId:callbackId];
    }
    
    void _requestToJoinGroup(int callbackId, const char* groupId, const char* message)
    {
        [[EMSdkLib sharedSdkLib] requestToJoinGroup:CreateNSString(groupId) withMessage:CreateNSString(message) callbackId:callbackId];
    }
    
    void _leaveGroup(int callbackId, const char* groupId)
    {
        [[EMSdkLib sharedSdkLib] leaveGroup:CreateNSString(groupId) callbackId:callbackId];
    }
    
    void _approveJoinGroupRequest(int callbackId, const char* groupId, const char* sender)
    {
        [[EMSdkLib sharedSdkLib] approveJoinGroupRequest:CreateNSString(groupId) sender:CreateNSString(sender) callbackId:callbackId];
    }
    
    void _declineJoinGroupRequest(int callbackId, const char* groupId, const char* sender, const char* reason)
    {
        [[EMSdkLib sharedSdkLib] declineJoinGroupRequest:CreateNSString(groupId) sender:CreateNSString(sender) reason:CreateNSString(reason) callbackId:callbackId];
    }

    void _acceptInvitationFromGroup(int callbackId, const char* groupId, const char* inviter)
    {
        [[EMSdkLib sharedSdkLib] acceptInvitationFromGroup:CreateNSString(groupId) inviter:CreateNSString(inviter) callbackId:callbackId];
    }
    
    void _declineInvitationFromGroup(int callbackId, const char* groupId, const char* inviter, const char* reason)
    {
        [[EMSdkLib sharedSdkLib] declineInvitationFromGroup:CreateNSString(groupId) inviter:CreateNSString(inviter) reason:CreateNSString(reason) callbackId:callbackId];
    }

}
