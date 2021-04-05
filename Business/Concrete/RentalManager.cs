using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;
        ICarDal _carDal;
        IUserDal _userDal;

        public RentalManager(IRentalDal rentalDal, ICarDal carDal, IUserDal userDal)
        {
            _rentalDal = rentalDal;
            _carDal = carDal;
            _userDal = userDal;
        }
        [CacheRemoveAspect("IRentalService.Get")]
        //[SecuredOperation("secretary,admin")]
        [ValidationAspect(typeof(RentalValidator))]
        public IResult Add(Rental rental)
        {
            var result = BusinessRules.Run(CheckCarAvailable(rental),
                CheckFindexScoreByUser(rental.UserId, rental.CarId));

            if (result != null)
            {
                return result;
            }

            _rentalDal.Add(rental);
            return new SuccessResult();
        }
        [CacheRemoveAspect("IRentalService.Get")]
        //[SecuredOperation("secretary,admin")]
        [ValidationAspect(typeof(RentalValidator))]
        public IResult Delete(Rental rental)
        {
            _rentalDal.Delete(rental);
            return new SuccessResult();
        }
        [CacheAspect]
        public IDataResult<Rental> GetById(int id)
        {
            return new SuccessDataResult<Rental>(_rentalDal.Get(r => r.RentalId == id));
        }
        [CacheAspect]
        public IDataResult<List<RentalDetailDto>> GetRentalDetails()
        {
            return new SuccessDataResult<List<RentalDetailDto>>(_rentalDal.GetRentalDetails());
        }
        [CacheAspect]
        public IDataResult<RentalDetailDto> GetRentalDetailsById(int id)
        {
            throw new System.NotImplementedException();
        }

        [CacheAspect]
        public IDataResult<List<Rental>> GetRentals()
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll());
        }
        [CacheRemoveAspect("IRentalService.Get")]
        //[SecuredOperation("secretary,admin")]
        [ValidationAspect(typeof(RentalValidator))]
        public IResult Update(Rental rental)
        {
            _rentalDal.Update(rental);
            return new SuccessResult();
        }

        private IResult CheckCarAvailable(Rental rental)
        {
            var result =
                _rentalDal.Get(r => (r.CarId == rental.CarId && r.ReturnDate == null)
            || (r.RentDate >= rental.RentDate && r.ReturnDate >= rental.RentDate));

            if (result != null)
            {
                return new ErrorResult(Messages.NotCarAvailable);
            }

            return new SuccessResult();
        }
        private IResult CheckFindexScoreByUser(int userId, int carId)
        {
            var car = _carDal.Get(c => c.CarId == carId);

            var user = _userDal.Get(c => c.Id == userId);

            var carScore = car.MinFindexScore;
            var userScore = user.FindexScore;

            if (userScore >= carScore)
            {
                return new SuccessResult();
            }
            return new ErrorResult(Messages.NotEnough);

        }
    }
}
