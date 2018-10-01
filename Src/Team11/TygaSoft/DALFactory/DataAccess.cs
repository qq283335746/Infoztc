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

        #region 电子商务

        public static IOrders CreateOrders()
        {
            string className = paths[0] + ".Orders";
            return (IOrders)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IOrderStatus CreateOrderStatus()
        {
            string className = paths[0] + ".OrderStatus";
            return (IOrderStatus)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IOrderItem CreateOrderItem()
        {
            string className = paths[0] + ".OrderItem";
            return (IOrderItem)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IProfiles CreateProfiles()
        {
            string className = paths[0] + ".Profiles";
            return (IProfiles)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static ICart CreateCart()
        {
            string className = paths[0] + ".Cart";
            return (ICart)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static ISupplier CreateSupplier()
        {
            string className = paths[0] + ".Supplier";
            return (ISupplier)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IMenuProduct CreateMenuProduct()
        {
            string className = paths[0] + ".MenuProduct";
            return (IMenuProduct)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static ICategoryPicture CreateCategoryPicture()
        {
            string className = paths[0] + ".CategoryPicture";
            return (ICategoryPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static ICategoryProduct CreateCategoryProduct()
        {
            string className = paths[0] + ".CategoryProduct";
            return (ICategoryProduct)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IBrandProduct CreateBrandProduct()
        {
            string className = paths[0] + ".BrandProduct";
            return (IBrandProduct)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static ICategory CreateCategory()
        {
            string className = paths[0] + ".Category";
            return (ICategory)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IBrand CreateBrand()
        {
            string className = paths[0] + ".Brand";
            return (IBrand)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static ICategoryBrand CreateCategoryBrand()
        {
            string className = paths[0] + ".CategoryBrand";
            return (ICategoryBrand)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #region 商品

        public static IProductStock CreateProductStock()
        {
            string className = paths[0] + ".ProductStock";
            return (IProductStock)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IProductImage CreateProductImage()
        {
            string className = paths[0] + ".ProductImage";
            return (IProductImage)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static IProductSize CreateProductSize()
        {
            string className = paths[0] + ".ProductSize";
            return (IProductSize)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static IProductSizePicture CreateProductSizePicture()
        {
            string className = paths[0] + ".ProductSizePicture";
            return (IProductSizePicture)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static IProductAttr CreateProductAttr()
        {
            string className = paths[0] + ".ProductAttr";
            return (IProductAttr)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IProductAttrTemplate CreateProductAttrTemplate()
        {
            string className = paths[0] + ".ProductAttrTemplate";
            return (IProductAttrTemplate)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static IProductItem CreateProductItem()
        {
            string className = paths[0] + ".ProductItem";
            return (IProductItem)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IProductDetail CreateProductDetail()
        {
            string className = paths[0] + ".ProductDetail";
            return (IProductDetail)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static IProduct CreateProduct()
        {
            string className = paths[0] + ".Product";
            return (IProduct)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        public static IProductPicture CreateProductPicture()
        {
            string className = paths[0] + ".ProductPicture";
            return (IProductPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static ISizePicture CreateSizePicture()
        {
            string className = paths[0] + ".SizePicture";
            return (ISizePicture)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #region 相册

        public static IPictureBrand CreatePictureBrand()
        {
            string className = paths[0] + ".PictureBrand";
            return (IPictureBrand)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IPictureCategory CreatePictureCategory()
        {
            string className = paths[0] + ".PictureCategory";
            return (IPictureCategory)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IPictureProduct CreatePictureProduct()
        {
            string className = paths[0] + ".PictureProduct";
            return (IPictureProduct)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IPictureProductSize CreatePictureProductSize()
        {
            string className = paths[0] + ".PictureProductSize";
            return (IPictureProductSize)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        #endregion

        #region HnztcDb

        public static IOrderRandom CreateOrderRandom()
        {
            string className = paths[0] + ".OrderRandom";
            return (IOrderRandom)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IUserSignIn CreateUserSignIn()
        {
            string className = paths[0] + ".UserSignIn";
            return (IUserSignIn)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static IPictureServiceItem CreatePictureServiceItem()
        {
            string className = paths[0] + ".PictureServiceItem";
            return (IPictureServiceItem)Assembly.Load(paths[1]).CreateInstance(className);
        } 
        public static IPictureServiceVote CreatePictureServiceVote()
        {
            string className = paths[0] + ".PictureServiceVote";
            return (IPictureServiceVote)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IPictureServiceLink CreatePictureServiceLink()
        {
            string className = paths[0] + ".PictureServiceLink";
            return (IPictureServiceLink)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IPictureServiceContent CreatePictureServiceContent()
        {
            string className = paths[0] + ".PictureServiceContent";
            return (IPictureServiceContent)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static IServiceUserPraise CreateServiceUserPraise()
        {
            string className = paths[0] + ".ServiceUserPraise";
            return (IServiceUserPraise)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IServiceUserVole CreateServiceUserVole()
        {
            string className = paths[0] + ".ServiceUserVole";
            return (IServiceUserVole)Assembly.Load(paths[1]).CreateInstance(className);
        } 
        public static IServicePicture CreateServicePicture()
        {
            string className = paths[0] + ".ServicePicture";
            return (IServicePicture)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IServiceUnion CreateServiceUnion()
        {
            string className = paths[0] + ".ServiceUnion";
            return (IServiceUnion)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IServiceItem CreateServiceItem()
        {
            string className = paths[0] + ".ServiceItem";
            return (IServiceItem)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IServiceVote CreateServiceVote()
        {
            string className = paths[0] + ".ServiceVote";
            return (IServiceVote)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IServiceLink CreateServiceLink()
        {
            string className = paths[0] + ".ServiceLink";
            return (IServiceLink)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IServiceContent CreateServiceContent()
        {
            string className = paths[0] + ".ServiceContent";
            return (IServiceContent)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static IAdvertisement CreateAdvertisement()
        {
            string className = paths[0] + ".Advertisement";
            return (IAdvertisement)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IAdBase CreateAdBase()
        {
            string className = paths[0] + ".AdBase";
            return (IAdBase)Assembly.Load(paths[1]).CreateInstance(className);
        } 
        public static IAdItem CreateAdItem()
        {
            string className = paths[0] + ".AdItem";
            return (IAdItem)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IAdItemLink CreateAdItemLink()
        {
            string className = paths[0] + ".AdItemLink";
            return (IAdItemLink)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IAdItemContent CreateAdItemContent()
        {
            string className = paths[0] + ".AdItemContent";
            return (IAdItemContent)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static IAdvertisementItem CreateAdvertisementItem()
        {
            string className = paths[0] + ".AdvertisementItem";
            return (IAdvertisementItem)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IAdvertisementLink CreateAdvertisementLink()
        {
            string className = paths[0] + ".AdvertisementLink";
            return (IAdvertisementLink)Assembly.Load(paths[1]).CreateInstance(className);
        }
        public static IAdvertisementPicture CreateAdvertisementPicture()
        {
            string className = paths[0] + ".AdvertisementPicture";
            return (IAdvertisementPicture)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        public static INotice CreateNotice()
        {
            string className = paths[0] + ".Notice";
            return (INotice)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IAnnouncement CreateAnnouncement()
        {
            string className = paths[0] + ".Announcement";
            return (IAnnouncement)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IContentType CreateContentType()
        {
            string className = paths[0] + ".ContentType";
            return (IContentType)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IContentDetail CreateContentDetail()
        {
            string className = paths[0] + ".ContentDetail";
            return (IContentDetail)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IContentPicture CreateContentPicture()
        {
            string className = paths[0] + ".ContentPicture";
            return (IContentPicture)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IPictureContent CreatePictureContent()
        {
            string className = paths[0] + ".PictureContent";
            return (IPictureContent)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IProvinceCity CreateProvinceCity()
        {
            string className = paths[0] + ".ProvinceCity";
            return (IProvinceCity)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static ISysEnum CreateSysEnum()
        {
            string className = paths[0] + ".SysEnum";
            return (ISysEnum)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IUserLevelProduce CreateUserLevelProduce()
        {
            string className = paths[0] + ".UserLevelProduce";
            return (IUserLevelProduce)Assembly.Load(paths[1]).CreateInstance(className);
        }

        public static IUserLevelView CreateUserLevelView()
        {
            string className = paths[0] + ".UserLevelView";
            return (IUserLevelView)Assembly.Load(paths[1]).CreateInstance(className);
        }

        #endregion

        public static IRole CreateRole()
        {
            string className = paths[0] + ".Role";
            return (IRole)Assembly.Load(paths[1]).CreateInstance(className);
        }

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

        public static ISysLog CreateSysLog()
        {
            string className = paths[0] + ".SysLog";
            return (ISysLog)Assembly.Load(paths[1]).CreateInstance(className);
        } 

        #endregion

    }
}
