using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using TygaSoft.IDAL;

namespace TygaSoft.DALFactory
{
    public sealed class DataAccess
    {
        private static readonly string[] paths = ConfigurationManager.AppSettings["WebDAL"].Split(',');

        #region 视频

        public static IHWTV CreateHWTV()
        {
            string className = paths[0] + ".HWTV";
            return (IHWTV)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static ITVProgram CreateTVProgram()
        {
            string className = paths[0] + ".TVProgram";
            return (ITVProgram)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        #region 知识竞猜

        public static IAnswerOption CreateAnswerOption()
        {
            string className = paths[0] + ".AnswerOption";
            return (IAnswerOption)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IQuestionBank CreateQuestionBank()
        {
            string className = paths[0] + ".QuestionBank";
            return (IQuestionBank)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IQuestionSubject CreateQuestionSubject()
        {
            string className = paths[0] + ".QuestionSubject";
            return (IQuestionSubject)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IActivityRelease CreateActivityRelease()
        {
            string className = paths[0] + ".ActivityRelease";
            return (IActivityRelease)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IActivityQuestionBank CreateActivityQuestionBank()
        {
            string className = paths[0] + ".ActivityQuestionBank";
            return (IActivityQuestionBank)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IAnswerStatistics CreateAnswerStatistics()
        {
            string className = paths[0] + ".AnswerStatistics";
            return (IAnswerStatistics)Assembly.Load(paths[1]).CreateInstance(className);
        }


        #endregion

        #region 彩票

        public static IQXCLotteryNumber CreateQXCLotteryNumber()
        {
            string className = paths[0] + ".QXCLotteryNumber";
            return (IQXCLotteryNumber)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        #region 话题

        public static ITopicSubject CreateTopicSubject()
        {
            string className = paths[0] + ".TopicSubject";
            return (ITopicSubject)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static ITopicPicture CreateTopicPicture()
        {
            string className = paths[0] + ".TopicPicture";
            return (ITopicPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static ITopicComment CreateTopicComment()
        {
            string className = paths[0] + ".TopicComment";
            return (ITopicComment)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static ICommunionPicture CreateCommunionPicture()
        {
            string className = paths[0] + ".CommunionPicture";
            return (ICommunionPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        #region 活动

        public static IActivityPictureNew CreateActivityPictureNew()
        {
            string className = paths[0] + ".ActivityPictureNew";
            return (IActivityPictureNew)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IActivityPlayerNew CreateActivityPlayerNew()
        {
            string className = paths[0] + ".ActivityPlayerNew";
            return (IActivityPlayerNew)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IPlayerPictureNew CreatePlayerPictureNew()
        {
            string className = paths[0] + ".PlayerPictureNew";
            return (IPlayerPictureNew)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IVoteToPlayerNew CreateVoteToPlayerNew()
        {
            string className = paths[0] + ".VoteToPlayerNew";
            return (IVoteToPlayerNew)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IActivitySubjectNew CreateActivitySubjectNew()
        {
            string className = paths[0] + ".ActivitySubjectNew";
            return (IActivitySubjectNew)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IVoteToPlayer CreateVoteToPlayer()
        {
            string className = paths[0] + ".VoteToPlayer";
            return (IVoteToPlayer)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IActivitySignUp CreateActivitySignUp()
        {
            string className = paths[0] + ".ActivitySignUp";
            return (IActivitySignUp)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IPlayerPicture CreatePlayerPicture()
        {
            string className = paths[0] + ".PlayerPicture";
            return (IPlayerPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IActivitySubject CreateActivitySubject()
        {
            string className = paths[0] + ".ActivitySubject";
            return (IActivitySubject)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IActivityPlayer CreateActivityPlayer()
        {
            string className = paths[0] + ".ActivityPlayer";
            return (IActivityPlayer)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IActivityPicture CreateActivityPicture()
        {
            string className = paths[0] + ".ActivityPicture";
            return (IActivityPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IInitItems CreateInitItems()
        {
            string className = paths[0] + ".InitItems";
            return (IInitItems)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IActivityPlayerPhotoPicture CreateActivityPlayerPhotoPicture()
        {
            string className = paths[0] + ".ActivityPlayerPhotoPicture";
            return (IActivityPlayerPhotoPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IActivityPhotoPicture CreateActivityPhotoPicture()
        {
            string className = paths[0] + ".ActivityPhotoPicture";
            return (IActivityPhotoPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        #region 摇奖

        public static IErnie CreateErnie()
        {
            string className = paths[0] + ".Ernie";
            return (IErnie)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IErnieItem CreateErnieItem()
        {
            string className = paths[0] + ".ErnieItem";
            return (IErnieItem)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IUserErnie CreateUserErnie()
        {
            string className = paths[0] + ".UserErnie";
            return (IUserErnie)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        #region 刮刮奖

        public static IActivityPrize CreateActivityPrize()
        {
            string className = paths[0] + ".ActivityPrize";
            return (IActivityPrize)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IPrizePicture CreatePrizePicture()
        {
            string className = paths[0] + ".PrizePicture";
            return (IPrizePicture)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IWinningRecord CreateWinningRecord()
        {
            string className = paths[0] + ".WinningRecord";
            return (IWinningRecord)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IPictureScratchLotto CreatePictureScratchLotto()
        {
            string className = paths[0] + ".PictureScratchLotto";
            return (IPictureScratchLotto)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        #region 刮刮奖定时任务
        public static IScratchLotto CreateScratchLotto()
        {
            string className = paths[0] + ".ScratchLotto";
            return (IScratchLotto)Assembly.Load(paths[1]).CreateInstance(className);
        }
        #endregion

        #region 新资讯广告

        public static IInformation CreateInformation()
        {
            string className = paths[0] + ".Information";
            return (IInformation)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IInformationAd CreateInformationAd()
        {
            string className = paths[0] + ".InformationAd";
            return (IInformationAd)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IPictureInformation CreatePictureInformation()
        {
            string className = paths[0] + ".PictureInformation";
            return (IPictureInformation)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IInformationPicture CreateInformationPicture()
        {
            string className = paths[0] + ".InformationPicture";
            return (IInformationPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IInformationAdPicture CreateInformationAdPicture()
        {
            string className = paths[0] + ".InformationAdPicture";
            return (IInformationAdPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        #region 启动页广告

        public static IAdvertPicture CreateAdvertPicture()
        {
            string className = paths[0] + ".AdvertPicture";
            return (IAdvertPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IAdvertSubject CreateAdvertSubject()
        {
            string className = paths[0] + ".AdvertSubject";
            return (IAdvertSubject)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IPictureAdStartup CreatePictureAdStartup()
        {
            string className = paths[0] + ".PictureAdStartup";
            return (IPictureAdStartup)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        #region 电子商务

        public static ISupplier CreateSupplier()
        {
            string className = paths[0] + ".Supplier";
            return (ISupplier)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        #region 推送管理

        public static IPushMsg CreatePushMsg()
        {
            string className = paths[0] + ".PushMsg";
            return (IPushMsg)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IPushUser CreatePushUser()
        {
            string className = paths[0] + ".PushUser";
            return (IPushUser)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        #region 系统

        public static IUserHeadPicture CreateUserHeadPicture()
        {
            string className = paths[0] + ".UserHeadPicture";
            return (IUserHeadPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IUserBase CreateUserBase()
        {
            string className = paths[0] + ".UserBase";
            return (IUserBase)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

    }
}
