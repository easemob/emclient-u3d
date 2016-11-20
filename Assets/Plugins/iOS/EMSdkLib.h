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
- (void) logout:(bool)flag;
- (void) sendTextMessage:(NSString *)content toUser:(NSString *)to callbackId:(int)callbackId chattype:(int)chattype;
- (void) sendFileMessage:(NSString *)path toUser:(NSString *)to callbackId:(int)callbackId chattype:(int)chattype;
- (NSString *) getAllContactsFromServer;
- (NSString *) getAllConversations;
- (NSString *) loadMessagesStartFromId:(NSString *)msgId fromUser:(NSString *)username pageSize:(int)size;
- (NSString *) getAllConversationMessage:(NSString *)fromUser;

- (void) createGroup:(NSString *)groupName desc:(NSString *)desc members:(NSString *)ms reason:(NSString *)reason maxUsers:(int)count type:(int)type callbackId:(int)cbId;
- (void) destroyGroup:(NSString *) aGroupId callbackId:(int) cbId;
- (void) updateGroupSubject:(NSString *) aSubject forGroup:(NSString *)aGroupId callbackId:(int)cbId;
- (void) updateGroupDescription:(NSString *) aDescription forGroup:(NSString *)aGroupId callbackId:(int)cbId;

- (NSString *)getJoinedGroups;
- (NSString *)getGroup:(NSString *)groupId;
- (void)getJoinedGroupsFromServer:(int)cbId;
- (void)getGroupSpecificationFromServerById:(NSString *)aGroupId includeMembersList:(BOOL)aIncludeMemberList callbackId:(int)cbId;
- (void)getGroupBlacklistFromServerById:(NSString *)aGroupId callbackId:(int)cbId;

- (void) addMembers:(NSString *)ms toGroup: (NSString *) aGroupId withMessage:(NSString *)message callbackId:(int) cbId;
- (void) removeMembers:(NSString *)ms fromGroup: (NSString *) aGroupId callbackId:(int) cbId;
- (void) blockMembers:(NSString *)ms fromGroup: (NSString *) aGroupId callbackId:(int) cbId;
- (void) unblockMembers:(NSString *)ms fromGroup: (NSString *) aGroupId callbackId:(int) cbId;

- (void) blockGroupMessage:(NSString *)aGroupId callbackId:(int) cbId;
- (void) unblockGroupMessage:(NSString *)aGroupId callbackId:(int) cbId;
- (int) getUnreadMsgCount:(NSString *)fromUser;
- (void) markAllMessagesAsRead:(NSString *)fromUser;
- (bool) deleteConversation:(NSString *)fromUser delHistory:(bool)flag;
- (void) removeMessage:(NSString *)fromUser messageId:(NSString *)msgId;

- (void) joinGroup:(NSString *)aGroupId callbackId:(int) cbId;
- (void) inviterUserToJoin:(NSString *) aGroupId;
- (void) leaveGroup:(NSString *)aGroupId callbackId:(int) cbId;

- (void) approveJoinGroupRequest:(NSString *)aGroupId sender:(NSString *) aUsername callbackId:(int) cbId;
- (void) declineJoinGroupRequest:(NSString *)aGroupId sender:(NSString *) aUsername reason:(NSString *)aReason callbackId:(int) cbId;
- (void) acceptInvitationFromGroup:(NSString *)aGroupId inviter:(NSString *) aUsername callbackId:(int) cbId;
- (void) declineInvitationFromGroup:(NSString *)aGroupId inviter:(NSString *) aUsername reason:(NSString *)aReason callbackId:(int) cbId;


@end
