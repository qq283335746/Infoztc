using System.ServiceModel;

namespace TygaSoft.WcfService
{
    [ServiceContract(Namespace = "TygaSoft.Services.HnztcTeamService")]
    public partial interface IHnztcTeam
    {
        #region 知识竞猜

        [OperationContract(Name = "GetQuestionList")]
        string GetQuestionList(string userId);

        [OperationContract(Name = "InsertUserAnswer")]
        string InsertUserAnswer(string userId, string questionSubjectId, string paperId, bool isTrue);

        #endregion

        #region 彩票

        [OperationContract(Name = "GetQXCLotteryList")]
        string GetQXCLotteryList(string userId, string qs, int qsCount, string sort);

        [OperationContract(Name = "GetQXCLotteryInfo")]
        string GetQXCLotteryInfo(string userId);

        #endregion

        #region 电视台

        [OperationContract(Name = "GetHWTVList")]
        string GetHWTVList();

        [OperationContract(Name = "GetTVProgramList")]
        string GetTVProgramList(string HWTVId);

        #endregion

        #region 话题
        [OperationContract(Name = "GetTopicList")]
        string GetTopicList(int pageIndex, int pageCount);

        [OperationContract(Name = "GetTopicInfo")]
        string GetTopicInfo(string topicID);

        [OperationContract(Name = "GetTopicCommentList")]
        string GetTopicCommentList(string topicID, int pageIndex, int pageCount);

        [OperationContract(Name = "AddTopicComment")]
        string AddTopicComment(string userId, string topicID, string content);
        #endregion

        #region 活动

        [OperationContract(Name = "GetActivityList")]
        string GetActivityList(int pageIndex, int pageCount);

        [OperationContract(Name = "GetActivityInfo")]
        string GetActivityInfo(string activityID, string userID);

        [OperationContract(Name = "GetActivityPlayerList")]
        string GetActivityPlayerList(string activityID, int pageIndex, int pageCount);

        [OperationContract(Name = "GetActivityPlayerInfo")]
        string GetActivityPlayerInfo(string playerID, string userID);

        [OperationContract(Name = "AddVoteToPlayer")]
        string AddVoteToPlayer(string playerID, string userID, int voteCount);

        [OperationContract(Name = "AddSignUpPlayer")]
        string AddSignUpPlayer(string LoginID, string ActivityId, string Remark);

        [OperationContract(Name = "UpdateActivityPlayerPicture")]
        string UpdateActivityPlayerPicture(string username, string imageBase64, string fileName);

        #endregion

        #region 活动（新）

        [OperationContract(Name = "GetActivityListNew")]
        string GetActivityListNew(int pageIndex, int pageCount);

        [OperationContract(Name = "GetActivityInfoNew")]
        string GetActivityInfoNew(string activityID, string userID, int pageIndex, int pageCount);

        [OperationContract(Name = "GetActivityPlayerListNew")]
        string GetActivityPlayerListNew(string activityID, string key, int pageIndex, int pageCount);

        [OperationContract(Name = "GetActivityPlayerInfoNew")]
        string GetActivityPlayerInfoNew(string playerID, string userID);

        [OperationContract(Name = "AddSignUpNew")]
        string AddSignUpNew(string userID, string activityID, string name, string age, string occupation, string phone, string location, string professional, string detailInfo);

        [OperationContract(Name = "AddSignUpImg")]
        string AddSignUpImg(string playerID, string userID, string fileName, string base64String, int sort);

        [OperationContract(Name = "AddVoteToPlayerNew")]
        string AddVoteToPlayerNew(string playerID, string userID);

        [OperationContract(Name = "SubmitScratchLotto")]
        string SubmitScratchLotto(string userID, string activityID);

        [OperationContract(Name = "SubmitWinningInfo")]
        string SubmitWinningInfo(string userID, string activityID, string prizeID, string mobilePhone);

        [OperationContract(Name = "GetMyWinningRecordList")]
        string GetMyWinningRecordList(string userID, string activityID, int pageIndex, int pageCount);

        [OperationContract(Name = "UpdateWinningMobilePhone")]
        string UpdateWinningMobilePhone(string userID, string winningID, string mobilePhone);
        #endregion

        #region 摇奖

        [OperationContract(Name = "IsExistErnieLatest")]
        string IsExistErnieLatest();

        #endregion

        #region 用户基本信息

        [OperationContract(Name = "GetUserInfo")]
        string GetUserInfo(string username);

        [OperationContract(Name = "UpdateHeadPicture")]
        string UpdateHeadPicture(string username, string imageBase64, string fileName);

        [OperationContract(Name = "UpdateUserBaseModel")]
        string UpdateUserBaseModel(string username, string nickname);

        #endregion

        #region 资讯
        [OperationContract(Name = "GetInformationList")]
        string GetInformationList(int pageIndex, int pageCount);

        [OperationContract(Name = "GetInformationInfo")]
        string GetInformationInfo(string informationID);


        [OperationContract(Name = "GetInformationAdInfo")]
        string GetInformationAdInfo(string informationAdID);

        #endregion

        #region 广告
        [OperationContract(Name = "GetAdvertInfo")]
        string GetAdvertInfo();
        #endregion
    }
}
