/*!
 *  @header EMClient.h
 *  @abstract SDK Client
 *  @author Hyphenate
 *  @version 3.00
 */

#import <Foundation/Foundation.h>

#import "EMClientDelegate.h"
#import "EMOptions.h"
//#import "EMPushOptions.h"
#import "EMError.h"

#import "IEMChatManager.h"
#import "IEMGroupManager.h"
//#import "IEMChatroomManager.h"

/*!
 *  SDK Client
 */
@interface EMClient : NSObject
{
//    EMPushOptions *_pushOptions;
}

/*!
 *  \~chinese 
 *  SDK版本号
 *
 *  \~english 
 *  SDK version
 */
@property (nonatomic, strong, readonly) NSString *version;

/*!
 *  \~chinese 
 *  当前登录账号
 *
 *  \~english 
 *  Current logged in user's username
 */
@property (nonatomic, strong, readonly) NSString *currentUsername;

/*!
 *  \~chinese 
 *  SDK属性
 *
 *  \~english
 *  SDK setting options
 */
@property (nonatomic, strong, readonly) EMOptions *options;

/*!
 *  \~chinese 
 *  推送设置
 *
 *  \~english 
 *  Apple Push Notification Service setting
 */
//@property (nonatomic, strong, readonly) EMPushOptions *pushOptions;

/*!
 *  \~chinese 
 *  聊天模块
 *
 *  \~english 
 *  Chat Management
 */
@property (nonatomic, strong, readonly) id<IEMChatManager> chatManager;

/*!
 *  \~chinese 
 *  好友模块
 *
 *  \~english 
 *  Contact Management
 */
//@property (nonatomic, strong, readonly) id<IEMContactManager> contactManager;

/*!
 *  \~chinese 
 *  群组模块
 *
 *  \~english 
 *  Group Management
 */
@property (nonatomic, strong, readonly) id<IEMGroupManager> groupManager;

/*!
 *  \~chinese 
 *  聊天室模块
 *
 *  \~english 
 *  Chat room Management
 */
//@property (nonatomic, strong, readonly) id<IEMChatroomManager> roomManager;

/*!
 *  \~chinese 
 *  SDK是否自动登录上次登录的账号
 *
 *  \~english
 *  If SDK will automatically log into with previously logged in session
 */
@property (nonatomic, readonly) BOOL isAutoLogin;

/*!
 *  \~chinese 
 *  用户是否已登录
 *
 *  \~english 
 *  If a user logged in
 */
@property (nonatomic, readonly) BOOL isLoggedIn;

/*!
 *  \~chinese 
 *  是否连上聊天服务器
 *
 *  \~english 
 *  Connection status to Hyphenate IM server
 */
@property (nonatomic, readonly) BOOL isConnected;

/*!
 *  \~chinese 
 *  获取SDK实例
 *
 *  \~english
 *  Get SDK singleton instance
 */
+ (instancetype)sharedClient;

#pragma mark - Delegate

/*!
 *  \~chinese 
 *  添加回调代理
 *
 *  @param aDelegate  要添加的代理
 *  @param aQueue     执行代理方法的队列
 *
 *  \~english 
 *  Add delegate
 *
 *  @param aDelegate  Delegate
 *  @param aQueue     The queue of calling delegate methods
 */
- (void)addDelegate:(id<EMClientDelegate>)aDelegate
      delegateQueue:(dispatch_queue_t)aQueue;

/*!
 *  \~chinese 
 *  移除回调代理
 *
 *  @param aDelegate  要移除的代理
 *
 *  \~english 
 *  Remove delegate
 *  
 *  @param aDelegate  Delegate
 */
- (void)removeDelegate:(id)aDelegate;

#pragma mark - Initialize SDK

/*!
 *  \~chinese 
 *  初始化sdk
 *
 *  @param aOptions  SDK配置项
 *
 *  @result 错误信息
 *
 *  \~english 
 *  Initialize the SDK
 *  
 *  @param aOptions  SDK setting options
 *
 *  @result Error
 */
- (EMError *)initializeSDKWithOptions:(EMOptions *)aOptions;

#pragma mark - Sync method

#pragma mark - Register

/*!
 *  \~chinese
 *  注册用户
 *
 *  同步方法，会阻塞当前线程. 不推荐使用，建议后台通过REST注册
 *
 *  @param aUsername  用户名
 *  @param aPassword  密码
 *
 *  @result 错误信息
 *
 *  \~english
 *  Register a new IM user
 *
 *  To enhance the reliability, registering new IM user through REST API from backend is highly recommended
 *
 *  @param aUsername  Username
 *  @param aPassword  Password
 *
 *  @result Error
 */
- (EMError *)registerWithUsername:(NSString *)aUsername
                         password:(NSString *)aPassword;

#pragma mark - Login

/*!
 *  \~chinese
 *  登录
 *
 *  同步方法，会阻塞当前线程
 *
 *  @param aUsername  用户名
 *  @param aPassword  密码
 *
 *  @result 错误信息
 *
 *  \~english
 *  Login
 *
 *  @param aUsername  Username
 *  @param aPassword  Password
 *
 *  @result Error
 */
- (EMError *)loginWithUsername:(NSString *)aUsername
                      password:(NSString *)aPassword;

#pragma makr - Logout

/*!
 *  \~chinese
 *  退出
 *
 *  同步方法，会阻塞当前线程
 *
 *  @param aIsUnbindDeviceToken 是否解除device token的绑定，解除绑定后设备不会再收到消息推送
 *         如果传入YES, 解除绑定失败，将返回error
 *
 *  @result 错误信息
 *
 *  \~english
 *  Logout
 *
 *  @param aIsUnbindDeviceToken Unbind device token to disable Apple Push Notification Service
 *
 *  @result Error
 */
- (EMError *)logout:(BOOL)aIsUnbindDeviceToken;

#pragma mark - Apns

/*!
 *  \~chinese
 *  绑定device token
 *
 *  同步方法，会阻塞当前线程
 *
 *  @param aDeviceToken  要绑定的token
 *
 *  @result 错误信息
 *
 *  \~english
 *  Device token binding is required for enabling Apple Push Notification Service
 *
 *  @param aDeviceToken  Device token to bind
 *
 *  @result Error
 */
//- (EMError *)bindDeviceToken:(NSData *)aDeviceToken;

/*!
 *  \~chinese
 *  设置推送消息显示的昵称
 *
 *  同步方法，会阻塞当前线程
 *
 *  @param aNickname  要设置的昵称
 *
 *  @result 错误信息
 *
 *  \~english
 *  Set display name for Apple Push Notification message
 *
 *  @param aNickname  Display name
 *
 *  @result Error
 */
//- (EMError *)setApnsNickname:(NSString *)aNickname;

/*!
 *  \~chinese 
 *  从服务器获取推送属性
 *
 *  同步方法，会阻塞当前线程
 *
 *  @param pError  错误信息
 *
 *  @result 推送属性
 *
 *  \~english
 *  Get Apple Push Notification Service options from the server
 *
 *  @param pError  Error
 *
 *  @result Apple Push Notification Service options
 */
//- (EMPushOptions *)getPushOptionsFromServerWithError:(EMError **)pError;

/*!
 *  \~chinese 
 *  更新推送设置到服务器
 *
 *  同步方法，会阻塞当前线程
 *
 *  @result 错误信息
 *
 *  \~english
 *  Update Apple Push Notification Service options to the server
 *
 *  @result Error
 */
//- (EMError *)updatePushOptionsToServer;

/*!
 *  \~chinese
 *  上传日志到服务器
 *
 *  同步方法，会阻塞当前线程
 *
 *  @result 错误信息
 *
 *  \~english
 *  Upload debugging log to server
 *
 *  @result Error
 */
- (EMError *)uploadLogToServer;

#pragma mark - Async method

/*!
 *  \~chinese
 *  注册用户
 *
 *  不推荐使用，建议后台通过REST注册
 *
 *  @param aUsername        用户名
 *  @param aPassword        密码
 *  @param aCompletionBlock 完成的回调
 *
 *  \~english
 *  Register a new IM user
 *
 *  To enhance the reliability, registering new IM user through REST API from backend is highly recommended
 *
 *  @param aUsername        Username
 *  @param aPassword        Password
 *  @param aCompletionBlock The callback block of completion
 *
 */
- (void)registerWithUsername:(NSString *)aUsername
                    password:(NSString *)aPassword
                  completion:(void (^)(NSString *aUsername, EMError *aError))aCompletionBlock;

/*!
 *  \~chinese
 *  登录
 *
 *  @param aUsername        用户名
 *  @param aPassword        密码
 *  @param aCompletionBlock 完成的回调
 *
 *  \~english
 *  Login
 *
 *  @param aUsername        Username
 *  @param aPassword        Password
 *  @param aCompletionBlock The callback block of completion
 *
 */
- (void)loginWithUsername:(NSString *)aUsername
                 password:(NSString *)aPassword
               completion:(void (^)(NSString *aUsername, EMError *aError))aCompletionBlock;

/*!
 *  \~chinese
 *  退出
 *
 *  @param aIsUnbindDeviceToken 是否解除device token的绑定，解除绑定后设备不会再收到消息推送
 *         如果传入YES, 解除绑定失败，将返回error
 *  @param aCompletionBlock 完成的回调
 *
 *  \~english
 *  Logout
 *
 *  @param aIsUnbindDeviceToken Unbind device token to disable the Apple Push Notification Service
 *  @param aCompletionBlock The callback block of completion
 *
 */
- (void)logout:(BOOL)aIsUnbindDeviceToken
    completion:(void (^)(EMError *aError))aCompletionBlock;

/*!
 *  \~chinese
 *  绑定device token
 *
 *  @param aDeviceToken     要绑定的token
 *  @param aCompletionBlock 完成的回调
 *
 *  \~english
 *  Device token binding is required to enable Apple push notification service
 *
 *  @param aDeviceToken     Device token to bind
 *  @param aCompletionBlock The callback block of completion
 */
//- (void)registerForRemoteNotificationsWithDeviceToken:(NSData *)aDeviceToken
//                                           completion:(void (^)(EMError *aError))aCompletionBlock;

/*!
 *  \~chinese
 *  设置推送的显示名
 *
 *  @param aDisplayName     推送显示名
 *  @param aCompletionBlock 完成的回调
 *
 *  \~english
 *  Set display name for the push notification
 *
 *  @param aDisplayName     Display name of push
 *  @param aCompletionBlock The callback block of completion
 *
 */
//- (void)updatePushNotifiationDisplayName:(NSString *)aDisplayName
//                              completion:(void (^)(NSString *aDisplayName, EMError *aError))aCompletionBlock;
/*!
 *  \~chinese
 *  从服务器获取推送属性
 *
 *  @param aCompletionBlock 完成的回调
 *
 *  \~english
 *  Get Apple Push Notification Service options from the server
 *
 *  @param aCompletionBlock The callback block of completion
 */
//- (void)getPushNotificationOptionsFromServerWithCompletion:(void (^)(EMPushOptions *aOptions, EMError *aError))aCompletionBlock;

/*!
 *  \~chinese
 *  更新推送设置到服务器
 *
 *  @param aCompletionBlock 完成的回调
 *
 *  \~english
 *  Update Apple Push Notification Service options to the server
 *
 *  @param aCompletionBlock The callback block of completion
 */
//- (void)updatePushNotificationOptionsToServerWithCompletion:(void (^)(EMError *aError))aCompletionBlock;

/*!
 *  \~chinese
 *  上传日志到服务器
 *
 *  @param aCompletionBlock 完成的回调
 *
 *  \~english
 *  Upload log to server
 *
 *  @param aCompletionBlock The callback block of completion
 */
- (void)uploadDebugLogToServerWithCompletion:(void (^)(EMError *aError))aCompletionBlock;

#pragma mark - iOS

/*!
 *  \~chinese
 *  iOS专用，数据迁移到SDK3.0
 *
 *  同步方法，会阻塞当前线程
 *
 *  升级到SDK3.0版本需要调用该方法，开发者需要等该方法执行完后再进行数据库相关操作
 *
 *  @result 是否迁移成功
 *
 *  \~english
 *  Migrate the IM database to the latest SDK version
 *
 *  @result Return YES for success
 */
//- (BOOL)migrateDatabaseToLatestSDK;

/*!
 *  \~chinese 
 *  iOS专用，程序进入后台时，需要调用此方法断开连接
 *
 *  @param aApplication  UIApplication
 *
 *  \~english
 *  Disconnect from server when app enters background
 *
 *  @param aApplication  UIApplication
 */
- (void)applicationDidEnterBackground:(id)aApplication;

/*!
 *  \~chinese 
 *  iOS专用，程序进入前台时，需要调用此方法进行重连
 *
 *  @param aApplication  UIApplication
 *
 *  \~english
 *  Re-connect to server when app enters foreground
 *
 *  @param aApplication  UIApplication
 */
- (void)applicationWillEnterForeground:(id)aApplication;

/*!
 *  \~chinese
 *  iOS专用，程序在前台收到APNs时，需要调用此方法
 *
 *  @param application  UIApplication
 *  @param userInfo     推送内容
 *
 *  \~english
 *  Need to call this method when APP receive APNs in foreground
 *
 *  @param application  UIApplication
 *  @param userInfo     Push content
 */
- (void)application:(id)application didReceiveRemoteNotification:(NSDictionary *)userInfo;

@end
