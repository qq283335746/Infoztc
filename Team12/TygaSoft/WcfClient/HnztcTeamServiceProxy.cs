﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.8689
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(Namespace="TygaSoft.Services.HnztcTeamService", ConfigurationName="IHnztcTeam")]
public interface IHnztcTeam
{
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetQuestionList", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetQuestionListResponse")]
    string GetQuestionList(string userId);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/InsertUserAnswer", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/InsertUserAnswerResponse")]
    string InsertUserAnswer(string userId, string questionSubjectId, string paperId, bool isTrue);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetQXCLotteryList", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetQXCLotteryListResponse")]
    string GetQXCLotteryList(string userId, string qs, int qsCount, string sort);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetQXCLotteryInfo", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetQXCLotteryInfoResponse")]
    string GetQXCLotteryInfo(string userId);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetHWTVList", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetHWTVListResponse")]
    string GetHWTVList();
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetTVProgramList", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetTVProgramListResponse")]
    string GetTVProgramList(string HWTVId);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetTopicList", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetTopicListResponse")]
    string GetTopicList(int pageIndex, int pageCount);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetTopicInfo", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetTopicInfoResponse")]
    string GetTopicInfo(string topicID);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetTopicCommentList", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetTopicCommentListResponse")]
    string GetTopicCommentList(string topicID, int pageIndex, int pageCount);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddTopicComment", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddTopicCommentResponse")]
    string AddTopicComment(string userId, string topicID, string content);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityList", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityListResponse")]
    string GetActivityList(int pageIndex, int pageCount);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityInfo", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityInfoResponse")]
    string GetActivityInfo(string activityID, string userID);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityPlayerList", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityPlayerListResponse")]
    string GetActivityPlayerList(string activityID, int pageIndex, int pageCount);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityPlayerInfo", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityPlayerInfoResponse")]
    string GetActivityPlayerInfo(string playerID, string userID);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddVoteToPlayer", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddVoteToPlayerResponse")]
    string AddVoteToPlayer(string playerID, string userID, int voteCount);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddSignUpPlayer", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddSignUpPlayerResponse")]
    string AddSignUpPlayer(string LoginID, string ActivityId, string Remark);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/UpdateActivityPlayerPicture", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/UpdateActivityPlayerPictureResponse" +
        "")]
    string UpdateActivityPlayerPicture(string username, string imageBase64, string fileName);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityListNew", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityListNewResponse")]
    string GetActivityListNew(int pageIndex, int pageCount);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityInfoNew", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityInfoNewResponse")]
    string GetActivityInfoNew(string activityID, string userID, int pageIndex, int pageCount);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityPlayerListNew", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityPlayerListNewResponse")]
    string GetActivityPlayerListNew(string activityID, string key, int pageIndex, int pageCount);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityPlayerInfoNew", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetActivityPlayerInfoNewResponse")]
    string GetActivityPlayerInfoNew(string playerID, string userID);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddSignUpNew", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddSignUpNewResponse")]
    string AddSignUpNew(string userID, string activityID, string name, string age, string occupation, string phone, string location, string professional, string detailInfo);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddSignUpImg", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddSignUpImgResponse")]
    string AddSignUpImg(string playerID, string userID, string fileName, string base64String, int sort);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddVoteToPlayerNew", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/AddVoteToPlayerNewResponse")]
    string AddVoteToPlayerNew(string playerID, string userID);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/SubmitScratchLotto", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/SubmitScratchLottoResponse")]
    string SubmitScratchLotto(string userID, string activityID);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/SubmitWinningInfo", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/SubmitWinningInfoResponse")]
    string SubmitWinningInfo(string userID, string activityID, string prizeID, string mobilePhone);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetMyWinningRecordList", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetMyWinningRecordListResponse")]
    string GetMyWinningRecordList(string userID, string activityID, int pageIndex, int pageCount);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/UpdateWinningMobilePhone", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/UpdateWinningMobilePhoneResponse")]
    string UpdateWinningMobilePhone(string userID, string winningID, string mobilePhone);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/IsExistErnieLatest", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/IsExistErnieLatestResponse")]
    string IsExistErnieLatest();
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetUserInfo", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetUserInfoResponse")]
    string GetUserInfo(string username);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/UpdateHeadPicture", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/UpdateHeadPictureResponse")]
    string UpdateHeadPicture(string username, string imageBase64, string fileName);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/UpdateUserBaseModel", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/UpdateUserBaseModelResponse")]
    string UpdateUserBaseModel(string username, string nickname);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetInformationList", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetInformationListResponse")]
    string GetInformationList(int pageIndex, int pageCount);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetInformationInfo", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetInformationInfoResponse")]
    string GetInformationInfo(string informationID);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetInformationAdInfo", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetInformationAdInfoResponse")]
    string GetInformationAdInfo(string informationAdID);
    
    [System.ServiceModel.OperationContractAttribute(Action="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetAdvertInfo", ReplyAction="TygaSoft.Services.HnztcTeamService/IHnztcTeam/GetAdvertInfoResponse")]
    string GetAdvertInfo();
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface IHnztcTeamChannel : IHnztcTeam, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class HnztcTeamClient : System.ServiceModel.ClientBase<IHnztcTeam>, IHnztcTeam
{
    
    public HnztcTeamClient()
    {
    }
    
    public HnztcTeamClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public HnztcTeamClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public HnztcTeamClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public HnztcTeamClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public string GetQuestionList(string userId)
    {
        return base.Channel.GetQuestionList(userId);
    }
    
    public string InsertUserAnswer(string userId, string questionSubjectId, string paperId, bool isTrue)
    {
        return base.Channel.InsertUserAnswer(userId, questionSubjectId, paperId, isTrue);
    }
    
    public string GetQXCLotteryList(string userId, string qs, int qsCount, string sort)
    {
        return base.Channel.GetQXCLotteryList(userId, qs, qsCount, sort);
    }
    
    public string GetQXCLotteryInfo(string userId)
    {
        return base.Channel.GetQXCLotteryInfo(userId);
    }
    
    public string GetHWTVList()
    {
        return base.Channel.GetHWTVList();
    }
    
    public string GetTVProgramList(string HWTVId)
    {
        return base.Channel.GetTVProgramList(HWTVId);
    }
    
    public string GetTopicList(int pageIndex, int pageCount)
    {
        return base.Channel.GetTopicList(pageIndex, pageCount);
    }
    
    public string GetTopicInfo(string topicID)
    {
        return base.Channel.GetTopicInfo(topicID);
    }
    
    public string GetTopicCommentList(string topicID, int pageIndex, int pageCount)
    {
        return base.Channel.GetTopicCommentList(topicID, pageIndex, pageCount);
    }
    
    public string AddTopicComment(string userId, string topicID, string content)
    {
        return base.Channel.AddTopicComment(userId, topicID, content);
    }
    
    public string GetActivityList(int pageIndex, int pageCount)
    {
        return base.Channel.GetActivityList(pageIndex, pageCount);
    }
    
    public string GetActivityInfo(string activityID, string userID)
    {
        return base.Channel.GetActivityInfo(activityID, userID);
    }
    
    public string GetActivityPlayerList(string activityID, int pageIndex, int pageCount)
    {
        return base.Channel.GetActivityPlayerList(activityID, pageIndex, pageCount);
    }
    
    public string GetActivityPlayerInfo(string playerID, string userID)
    {
        return base.Channel.GetActivityPlayerInfo(playerID, userID);
    }
    
    public string AddVoteToPlayer(string playerID, string userID, int voteCount)
    {
        return base.Channel.AddVoteToPlayer(playerID, userID, voteCount);
    }
    
    public string AddSignUpPlayer(string LoginID, string ActivityId, string Remark)
    {
        return base.Channel.AddSignUpPlayer(LoginID, ActivityId, Remark);
    }
    
    public string UpdateActivityPlayerPicture(string username, string imageBase64, string fileName)
    {
        return base.Channel.UpdateActivityPlayerPicture(username, imageBase64, fileName);
    }
    
    public string GetActivityListNew(int pageIndex, int pageCount)
    {
        return base.Channel.GetActivityListNew(pageIndex, pageCount);
    }
    
    public string GetActivityInfoNew(string activityID, string userID, int pageIndex, int pageCount)
    {
        return base.Channel.GetActivityInfoNew(activityID, userID, pageIndex, pageCount);
    }
    
    public string GetActivityPlayerListNew(string activityID, string key, int pageIndex, int pageCount)
    {
        return base.Channel.GetActivityPlayerListNew(activityID, key, pageIndex, pageCount);
    }
    
    public string GetActivityPlayerInfoNew(string playerID, string userID)
    {
        return base.Channel.GetActivityPlayerInfoNew(playerID, userID);
    }
    
    public string AddSignUpNew(string userID, string activityID, string name, string age, string occupation, string phone, string location, string professional, string detailInfo)
    {
        return base.Channel.AddSignUpNew(userID, activityID, name, age, occupation, phone, location, professional, detailInfo);
    }
    
    public string AddSignUpImg(string playerID, string userID, string fileName, string base64String, int sort)
    {
        return base.Channel.AddSignUpImg(playerID, userID, fileName, base64String, sort);
    }
    
    public string AddVoteToPlayerNew(string playerID, string userID)
    {
        return base.Channel.AddVoteToPlayerNew(playerID, userID);
    }
    
    public string SubmitScratchLotto(string userID, string activityID)
    {
        return base.Channel.SubmitScratchLotto(userID, activityID);
    }
    
    public string SubmitWinningInfo(string userID, string activityID, string prizeID, string mobilePhone)
    {
        return base.Channel.SubmitWinningInfo(userID, activityID, prizeID, mobilePhone);
    }
    
    public string GetMyWinningRecordList(string userID, string activityID, int pageIndex, int pageCount)
    {
        return base.Channel.GetMyWinningRecordList(userID, activityID, pageIndex, pageCount);
    }
    
    public string UpdateWinningMobilePhone(string userID, string winningID, string mobilePhone)
    {
        return base.Channel.UpdateWinningMobilePhone(userID, winningID, mobilePhone);
    }
    
    public string IsExistErnieLatest()
    {
        return base.Channel.IsExistErnieLatest();
    }
    
    public string GetUserInfo(string username)
    {
        return base.Channel.GetUserInfo(username);
    }
    
    public string UpdateHeadPicture(string username, string imageBase64, string fileName)
    {
        return base.Channel.UpdateHeadPicture(username, imageBase64, fileName);
    }
    
    public string UpdateUserBaseModel(string username, string nickname)
    {
        return base.Channel.UpdateUserBaseModel(username, nickname);
    }
    
    public string GetInformationList(int pageIndex, int pageCount)
    {
        return base.Channel.GetInformationList(pageIndex, pageCount);
    }
    
    public string GetInformationInfo(string informationID)
    {
        return base.Channel.GetInformationInfo(informationID);
    }
    
    public string GetInformationAdInfo(string informationAdID)
    {
        return base.Channel.GetInformationAdInfo(informationAdID);
    }
    
    public string GetAdvertInfo()
    {
        return base.Channel.GetAdvertInfo();
    }
}
