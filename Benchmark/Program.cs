using AutoMapper;
using Benchmark;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FluentValidation;
using FluentValidation.Results;
using Riok.Mapperly.Abstractions;
using System;
using System.Runtime.ConstrainedExecution;
using static BenchmarkTest;

//static void InitializeAutomapper()
//{
//    AutoMapper.Mapper.CreateMap<RequestDto, Input>();
//}

BenchmarkRunner.Run<BenchmarkTest>();
//var list = StaticDataSource.ReturnStudentList();
//Console.WriteLine(list.Count);

#region Domain Entities Benchmark Test

[Mapper]
public partial class InputMapper
{
    //public Input MapRequestToInput(RequestDto requestDto, int facilityId,)
    //{

    //}

    [MapperIgnoreSource(nameof(RequestDto.IgnoreThisNumber))]
    public partial Input RequestToInput(RequestDto requestDto);
    public partial List<Input> ListRequestToInputList(List<RequestDto> requestDtoList);
    public partial Input1 ReqToInput(Req1 r1);
}

[MemoryDiagnoser(true)]
public class BenchmarkTest
{
    private readonly IMapper _mapper;
    private readonly InputMapper _inputMapper;
    public List<RequestDto> requestDtos { get; set; }

    public BenchmarkTest()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RequestDto, Input>();
            //.ForMember(x => x.IsId1, s => s.MapFrom(ss => ss.Id == 1 ? true : false))
            //.ForMember(x => x.IsPasswordNullOrEmpty, s => s.MapFrom(ss => ss.Password == "" ? true : false))
            //.ForMember(x => x.HasEmail, s => s.MapFrom(ss => ss.Email != "" ? true : false))
        });

        _inputMapper = new InputMapper();

        _mapper = config.CreateMapper();

        #region Liste obje 
        requestDtos = new List<RequestDto>();

        RequestDto requestDto = new RequestDto()
        {
            Author = "Author",
            Description = "Description",
            Email = "Email",
            Id = 1,
            Name = "Name",
            Password = "Password",
            Username = "Username"
        };


        for (int i = 0; i < 50; i++)
        {
            requestDtos.Add(requestDto);
        }
        #endregion
    }

    //[Benchmark]
    //public List<Input> AutoMapperList()
    //{
    //    var inputs = _mapper.Map<List<Input>>(this.requestDtos);

    //    return inputs;
    //}

    //[Benchmark]
    //public List<Input> NotAutoMapperList()
    //{
    //    List<Input> inputs = new List<Input>();

    //    foreach (var requestDto in this.requestDtos)
    //    {
    //        inputs.Add(new Input()
    //        {
    //            Author = requestDto.Author,
    //            Description = requestDto.Description,
    //            Email = requestDto.Email,
    //            Id = requestDto.Id,
    //            Name = requestDto.Name,
    //            Password = requestDto.Password,
    //            Username = requestDto.Username
    //        });
    //    }

    //    return inputs;
    //}


    //[Benchmark]
    //public List<Input> MapperlyList() 
    //{
    //    List<Input> dto = _inputMapper.ListRequestToInputList(this.requestDtos);
    //    return dto;
    //}

    [Benchmark]
    public Input AutoMapper()
    {
        RequestDto requestDto = new RequestDto()
        {
            Author = "Author",
            Description = "Description",
            Email = "Email",
            Id = 1,
            Name = "Name",
            Password = "Password",
            Username = "Username"
        };

        var inputs = _mapper.Map<Input>(requestDto);

        return inputs;
    }


    [Benchmark]
    public Input NotAutoMapper()
    {
        //List<Input> inputs = new List<Input>();

        //foreach (var requestDto in this.requestDtos)
        //{
        //    inputs.Add();
        //}

        RequestDto requestDto = new RequestDto()
        {
            Author = "Author",
            Description = "Description",
            Email = "Email",
            Id = 1,
            Name = "Name",
            Password = "Password",
            Username = "Username",
            IgnoreThisNumber = 236
        };

        Input input = new Input()
        {
            Author = requestDto.Author,
            Description = requestDto.Description,
            Email = requestDto.Email,
            Id = requestDto.Id,
            Name = requestDto.Name,
            Password = requestDto.Password,
            Username = requestDto.Username,
            //HasEmail = requestDto.Email != "" ? true : false,
            //IsId1 = requestDto.Id == 1 ? true : false,
            //IsPasswordNullOrEmpty = requestDto.Password == "" ? true : false
        };

        return input;
    }

    [Benchmark]
    public Input Mapperly()
    {
        RequestDto requestDto = new RequestDto()
        {
            Author = "Author",
            Description = "Description",
            Email = "Email",
            Id = 1,
            Name = "Name",
            Password = "Password",
            Username = "Username",
            IgnoreThisNumber = 236
        };

        Input dto = _inputMapper.RequestToInput(requestDto);
        return dto;
    }

    //[Benchmark]
    //public Input1 Mapperly1()
    //{
    //    Req1 req1 = new Req1()
    //    {
    //        Id = 11,
    //        Name = "sadsaddas",
    //        Password = "asffasfsa",
    //    };

    //    Input1 input1 = _inputMapper.ReqToInput(req1);
    //    return input1;
    //}

    public class Req1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class Input1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }


    public class RequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public int IgnoreThisNumber { get; set; }
    }

    public class Input
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public bool IsId1 { get; set; }
        public bool HasEmail { get; set; }
        public bool IsPasswordNullOrEmpty { get; set; }
    }

    //public List<int> OldSchoolReturn()
    //{
    //    List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    //    List<int> newList = new List<int>();

    //    foreach (var number in numbers)
    //    {
    //        newList.Add(number);
    //    }

    //    return newList;
    //}


    //[Benchmark]
    //public IEnumerable<int> YieldReturn()
    //{
    //    List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    //    foreach (var number in numbers)
    //    {
    //        yield return number;
    //    }
    //}
}

#region Domains
public sealed class SealedStockTransaction
{
    public Guid Id { get; private set; }
    public string FacilityId { get; private set; }
    public string TransactionId { get; private set; }
    public int TransactionLineId { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public Guid? OutWarehouseId { get; private set; }
    public Guid? InWarehouseId { get; private set; }
    public Guid ItemId { get; private set; }
    public string LotSerialNumber { get; private set; }
    public Guid? UnitId { get; private set; }
    public int UserId { get; private set; }
    public double Quantity { get; private set; }
    public int TransactionTypeId { get; private set; }
    public string PatientId { get; private set; }
    public int? ChargeLineNo { get; private set; }
    public DateTime? AcceptDate { get; private set; }
    public bool IsAccepted { get; private set; }
    public int? AcceptUser { get; private set; }
    public string Explanation { get; private set; }
    public Guid? RelatedStockTransactionId { get; private set; }
    public decimal? AverageCost { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public int OutQuantity { get; private set; }
    public int InQuantity { get; private set; }
    public int OutTotalQuantity { get; private set; }
    public int InTotalQuantity { get; private set; }
    public string ReasonKey { get; private set; }
    public string PatientRn { get; private set; }
    public Guid? RequestDetailId { get; private set; }

    public void SetAcceptedPropertyTrue()
    {
        IsAccepted = true;
    }

    public void AcceptStockTransaction(int acceptUser)
    {
        AcceptDate = DateTime.Now;
        AcceptUser = acceptUser;
        IsAccepted = true;
    }
    public static class Factory
    {
        public static SealedStockTransaction Create(string facilityId, string transactionId, int transactionLineId, Guid? outWarehouseId, Guid? inWarehouseId, Guid itemId, string lotSerialNumber, int userId, double quantity, int transactionTypeId, int? acceptUser, DateTime? expiryDate, bool isAccepted, Guid? requestDetailId, string reasonKey = null, string explanation = null, Guid? relatedStockTransactionId = null)
        {
            SealedStockTransaction stockTransaction = new SealedStockTransaction();
            stockTransaction.Id = Guid.NewGuid();
            stockTransaction.FacilityId = facilityId;
            stockTransaction.TransactionId = transactionId;
            stockTransaction.TransactionLineId = transactionLineId;
            stockTransaction.TransactionDate = DateTime.Now;
            stockTransaction.OutWarehouseId = outWarehouseId;
            stockTransaction.InWarehouseId = inWarehouseId;
            stockTransaction.ItemId = itemId;
            stockTransaction.LotSerialNumber = lotSerialNumber?.ToUpper();
            stockTransaction.UserId = userId;
            stockTransaction.Quantity = quantity;
            stockTransaction.TransactionTypeId = transactionTypeId;
            stockTransaction.AcceptUser = acceptUser;
            stockTransaction.ExpiryDate = expiryDate;
            stockTransaction.IsAccepted = isAccepted;
            stockTransaction.ReasonKey = reasonKey;
            stockTransaction.Explanation = explanation;
            stockTransaction.RequestDetailId = requestDetailId;
            stockTransaction.RelatedStockTransactionId = relatedStockTransactionId;
            return stockTransaction;
        }
    }
}

public class StockTransaction
{
    public Guid Id { get; private set; }
    public string FacilityId { get; private set; }
    public string TransactionId { get; private set; }
    public int TransactionLineId { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public Guid? OutWarehouseId { get; private set; }
    public Guid? InWarehouseId { get; private set; }
    public Guid ItemId { get; private set; }
    public string LotSerialNumber { get; private set; }
    public Guid? UnitId { get; private set; }
    public int UserId { get; private set; }
    public double Quantity { get; private set; }
    public int TransactionTypeId { get; private set; }
    public string PatientId { get; private set; }
    public int? ChargeLineNo { get; private set; }
    public DateTime? AcceptDate { get; private set; }
    public bool IsAccepted { get; private set; }
    public int? AcceptUser { get; private set; }
    public string Explanation { get; private set; }
    public Guid? RelatedStockTransactionId { get; private set; }
    public decimal? AverageCost { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public int OutQuantity { get; private set; }
    public int InQuantity { get; private set; }
    public int OutTotalQuantity { get; private set; }
    public int InTotalQuantity { get; private set; }
    public string ReasonKey { get; private set; }
    public string PatientRn { get; private set; }
    public Guid? RequestDetailId { get; private set; }

    public void SetAcceptedPropertyTrue()
    {
        IsAccepted = true;
    }

    public void AcceptStockTransaction(int acceptUser)
    {
        AcceptDate = DateTime.Now;
        AcceptUser = acceptUser;
        IsAccepted = true;
    }
    public static class Factory
    {
        public static StockTransaction Create(string facilityId, string transactionId, int transactionLineId, Guid? outWarehouseId, Guid? inWarehouseId, Guid itemId, string lotSerialNumber, int userId, double quantity, int transactionTypeId, int? acceptUser, DateTime? expiryDate, bool isAccepted, Guid? requestDetailId, string reasonKey = null, string explanation = null, Guid? relatedStockTransactionId = null)
        {
            StockTransaction stockTransaction = new StockTransaction();
            stockTransaction.Id = Guid.NewGuid();
            stockTransaction.FacilityId = facilityId;
            stockTransaction.TransactionId = transactionId;
            stockTransaction.TransactionLineId = transactionLineId;
            stockTransaction.TransactionDate = DateTime.Now;
            stockTransaction.OutWarehouseId = outWarehouseId;
            stockTransaction.InWarehouseId = inWarehouseId;
            stockTransaction.ItemId = itemId;
            stockTransaction.LotSerialNumber = lotSerialNumber?.ToUpper();
            stockTransaction.UserId = userId;
            stockTransaction.Quantity = quantity;
            stockTransaction.TransactionTypeId = transactionTypeId;
            stockTransaction.AcceptUser = acceptUser;
            stockTransaction.ExpiryDate = expiryDate;
            stockTransaction.IsAccepted = isAccepted;
            stockTransaction.ReasonKey = reasonKey;
            stockTransaction.Explanation = explanation;
            stockTransaction.RequestDetailId = requestDetailId;
            stockTransaction.RelatedStockTransactionId = relatedStockTransactionId;
            return stockTransaction;
        }
    }
}
#endregion

#endregion

#region AppService Benchmark 

//public class BenchmarkTest
//{
//    private IAppService _appService;

//    [Benchmark]
//    public int NotSealedAppService()
//    {
//        _appService = new NotSealedAppService();
//        int calculated = _appService.Calculate(2, 2);
//        int mult = _appService.Multiply(2, 2);
//        int div = _appService.Divide(2, 2);

//        return calculated + mult + div;
//    }

//    [Benchmark]
//    public int SealedAppService()
//    {
//        _appService = new SealedAppService();
//        int calculated = _appService.Calculate(2, 2);
//        int mult = _appService.Multiply(2, 2);
//        int div = _appService.Divide(2, 2);

//        return calculated + mult + div;
//    }
//}

public class NotSealedAppService : IAppService
{
    public int Calculate(int x, int y)
    {
        return x + y;
    }

    public int Divide(int x, int y)
    {
        return x / y;
    }

    public int Multiply(int x, int y)
    {
        return x * y;
    }
}

public sealed class SealedAppService : IAppService
{
    public int Calculate(int x, int y)
    {
        return x + y;
    }

    public int Divide(int x, int y)
    {
        return x / y;
    }

    public int Multiply(int x, int y)
    {
        return x * y;
    }
}

public interface IAppService
{
    int Calculate(int x, int y);
    int Multiply(int x, int y);
    int Divide(int x, int y);
}

#endregion

#region Fluent Validation Benchmark
//public class BenchmarkTest
//{
//    private IAStoreValidator _validator;
//    private readonly CreateTransferMainRequestValidation _currentValidator = new();
//    private readonly SealedValidation _sealedValidator = new();

//    [Benchmark]
//    public IAStoreValidationResult Current()
//    {
//        CreateTransferMainRequestDto x = new();
//        return _validator.Validate(_currentValidator, x);
//    }

//    [Benchmark]
//    public IAStoreValidationResult Sealed()
//    {
//        CreateTransferMainRequestDto x = new();
//        return _validator.Validate(_sealedValidator, x);
//    }
//}

#region Validator

public interface IAStoreValidationResult
{
    bool IsValid { get; }
    string Errors { get; }
}

public interface IAStoreValidator
{
    IAStoreValidationResult Validate<T, TModel>(T _validator, TModel entity);
}

public class AStoreValidationResult : IAStoreValidationResult
{
    public AStoreValidationResult(bool isValid, string _errors)
    {
        IsValid = isValid;
        Errors = _errors;
    }

    public bool IsValid { get; private set; }
    public string Errors { get; private set; }

}

public sealed class MMFluentValidator<T, TModel> : IAStoreValidator
{
    public IAStoreValidationResult Validate<T1, TModel>(T1 _validator, TModel entity)
    {
        ValidationContext<TModel> validationContext = new ValidationContext<TModel>(entity);

        var validator = (IValidator)_validator;

        var result = validator.Validate(validationContext);

        AStoreValidationResult astoreValidationResult = new AStoreValidationResult(result.IsValid, ConvertToAstoreErrors(result));

        return astoreValidationResult;
    }

    private static string ConvertToAstoreErrors(ValidationResult result)
    {
        var splitted = result.ToString(";");

        var errorDictionary = String.Concat(result.Errors);

        return splitted;
    }
}

public class CreateTransferMainRequestValidation : AbstractValidator<CreateTransferMainRequestDto>
{
    public CreateTransferMainRequestValidation()
    {
        RuleFor(x => x.SourceWarehouseId).NotNull().WithMessage("");
        RuleFor(x => x.TargetWarehouseId).NotNull().WithMessage("");
    }
}



public sealed class SealedValidation : AbstractValidator<CreateTransferMainRequestDto>
{
    public SealedValidation()
    {
        RuleFor(x => x.SourceWarehouseId).NotNull().WithMessage("");
        RuleFor(x => x.TargetWarehouseId).NotNull().WithMessage("");
    }
}

#endregion

#region DTO

public class CreateTransferMainRequestDto
{
    public Guid SourceWarehouseId { get; set; } = Guid.NewGuid();
    public Guid TargetWarehouseId { get; set; } = Guid.NewGuid();
}

#endregion
#endregion

#region Calculator Benchmark
//public class BenchmarkTest
//{
//    private readonly SealedClass _sealedClass = new();
//    private readonly NonSealedClass _nonSealedClass = new();


//    [Benchmark]
//    public int SealedClassBenchmark()
//    {
//        return _sealedClass.Calculate(2, 2);
//    }

//    [Benchmark]
//    public int NonSealedClassBenchmark()
//    {
//        return _nonSealedClass.Calculate(2, 2);
//    }
//}

//public sealed class SealedClass
//{
//    public int Calculate(int x, int y)
//    {
//        return x + y;
//    }
//} 

//public class NonSealedClass
//{
//    public int Calculate(int x, int y)
//    {
//        return x + y;
//    }
//}
#endregion

#region Domain Benchmark

//private SealedStockTransaction _sealedClass = new();
//private StockTransaction _nonSealedClass = new();

//[Benchmark]
//public void Current()
//{
//    for (int i = 0; i < 100; i++)
//    {
//        _nonSealedClass = StockTransaction.Factory.Create("1", "1", 1, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1", 1, 1, 1, 1, DateTime.Today, true, Guid.NewGuid());

//        _nonSealedClass.AcceptStockTransaction(1);

//        _nonSealedClass.SetAcceptedPropertyTrue();
//    }
//}

//[Benchmark]
//public void Sealed()
//{
//    for (int i = 0; i < 100; i++)
//    {
//        _sealedClass = SealedStockTransaction.Factory.Create("1", "1", 1, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1", 1, 1, 1, 1, DateTime.Today, true, Guid.NewGuid());

//        _sealedClass.AcceptStockTransaction(1);

//        _sealedClass.SetAcceptedPropertyTrue();
//    }
//}

#endregion