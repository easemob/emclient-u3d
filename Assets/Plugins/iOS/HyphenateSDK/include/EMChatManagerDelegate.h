/*!
 *  \~chinese
 *  @header EMChatManagerDelegate.h
 *  @abstract 此协议定义了聊天相关的回调
 *  @author Hyphenate
 *  @version 3.00
 *
 *  \~english
 *  @header EMChatManagerDelegate.h
 *  @abstract This protocol defines chat related callbacks
 *  @author Hyphenate
 *  @version 3.00
 */

#import <Foundation/Foundation.h>

@class EMMessage;
@class EMError;

/*!
 *  \~chinese
 *  聊天相关回调
 *
 *  \~english
 *  Chat related callbacks
 */
@protocol EMChatManagerDelegate <NSObject>

@optional

#pragma mark - Conversation

/*!
 *  \~chinese
 *  会话列表发生变化
 *
 *  @param aConversationList  会话列表<EMConversation>
 *
 *  \~english
 *  Delegate method will be invoked when the conversation list has changed
 *
 *  @param aConversationList  Conversation list<EMConversation>
 */
- (void)conversationListDidUpdate:(NSArray *)aConversationList;

#pragma mark - Message

/*!
 *  \~chinese
 *  收到消息
 *
 *  @param aMessages  消息列表<EMMessage>
 *
 *  \~english
 *  Delegate method will be invoked when receiving new messages
 *
 *  @param aMessages  Receivecd message list<EMMessage>
 */
- (void)messagesDidReceive:(NSArray *)aMessages;

/*!
 *  \~chinese
 *  收到Cmd消息
 *
 *  @param aCmdMessages  Cmd消息列表<EMMessage>
 *
 *  \~english
 *  Delegate method will be invoked when receiving command messages
 *
 *  @param aCmdMessages  Command message list<EMMessage>
 */
- (void)cmdMessagesDidReceive:(NSArray *)aCmdMessages;

/*!
 *  \~chinese
 *  收到已读回执
 *
 *  @param aMessages  已读消息列表<EMMessage>
 *
 *  \~english
 *   Delegate method will be invoked when receiving read acknowledgements for message list
 *
 *  @param aMessages  Acknowledged message list<EMMessage>
 */
- (void)messagesDidRead:(NSArray *)aMessages;

/*!
 *  \~chinese
 *  收到消息送达回执
 *
 *  @param aMessages  送达消息列表<EMMessage>
 *
 *  \~english
 * Delegate method will be invoked when receiving deliver acknowledgements for message list
 *
 *  @param aMessages  Acknowledged message list<EMMessage>
 */
- (void)messagesDidDeliver:(NSArray *)aMessages;

/*!
 *  \~chinese
 *  消息状态发生变化
 *
 *  @param aMessage  状态发生变化的消息
 *  @param aError    出错信息
 *
 *  \~english
 *  Delegate method will be invoked when message status has changed
 *
 *  @param aMessage  Message whose status has changed
 *  @param aError    Error info
 */
- (void)messageStatusDidChange:(EMMessage *)aMessage
                         error:(EMError *)aError;

/*!
 *  \~chinese
 *  消息附件状态发生改变
 *
 *  @param aMessage  附件状态发生变化的消息
 *  @param aError    错误信息
 *
 *  \~english
 *  Delegate method will be invoked when message attachment status has changed
 *
 *  @param aMessage  Message attachment status has changed
 *  @param aError    Error
 */
- (void)messageAttachmentStatusDidChange:(EMMessage *)aMessage
                                   error:(EMError *)aError;

@end
