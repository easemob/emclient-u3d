//
//  EMSdkLib.h
//  easemob
//
//  Created by lzj on 13/11/2016.
//  Copyright © 2016 EaseMob. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "EMSDK.h"

@interface EMSdkLib : NSObject <EMClientDelegate, EMChatManagerDelegate,EMGroupManagerDelegate>

+ (instancetype) sharedSdkLib;

- (void) initializeSDK:(NSString *) appKey;
- (void)applicationDidEnterBackground:(UIApplication *)application;
- (void)applicationWillEnterForeground:(UIApplication *)application;

- (int) createAccount:(NSString *)username withPwd: (NSString *)password;
- (void) login:(NSString *)username withPwd: (NSString *)password;
- (void) logout:(BOOL)flag;
- (void) sendTextMessage:(NSString *)content toUser:(NSString *)to callbackId:(int)callbackId chattype:(int)chattype ext:(NSString*) ext;
- (void) sendFileMessage:(NSString *)path toUser:(NSString *)to callbackId:(int)callbackId chattype:(int)chattype ext:(NSString*) ext;
- (NSString *) getAllContactsFromServer;
- (NSString *) getAllConversationMessage:(NSString *)fromUser;
- (NSString *) getAllConversations;
- (NSString *) getLatestMessage:(NSString *)fromUser;
- (NSString *) loadMessagesStartFromId:(NSString *)msgId fromUser:(NSString *)username pageSize:(int)size;

- (void) createGroup:(NSString *)groupName desc:(NSString *)desc members:(NSString *)ms reason:(NSString *)reason maxUsers:(int)count type:(int)type callbackId:(int)cbId;

- (NSString *)getGroup:(NSString *)groupId;
- (void)getJoinedGroupsFromServer:(int)cbId;

- (void) addMembers:(NSString *)ms toGroup: (NSString *) aGroupId withMessage:(NSString *)message callbackId:(int) cbId;

- (BOOL) deleteConversation:(NSString *)fromUser delHistory:(BOOL)flag;
- (void) removeMessage:(NSString *)fromUser messageId:(NSString *)msgId;

- (void) joinGroup:(NSString *)aGroupId callbackId:(int) cbId;
- (void) leaveGroup:(NSString *)aGroupId callbackId:(int) cbId;

- (void) downloadAttachmentFrom:(NSString *)username messageId:(NSString *)msgId callbackId:(int)cbId;
- (NSString *)getConversation:(NSString *)cid type:(int)type createIfNotExists:(BOOL)createIfNotExists;
- (void) deleteMessagesAsExitGroup:(BOOL)del;
- (void) isAutoAcceptGroupInvitation:(BOOL)isAuto;
- (void) isSortMessageByServerTime:(BOOL)isSort;
- (void) requireDeliveryAck:(BOOL)isReq;

@end
