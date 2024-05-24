using AppliComptaApi.Models;
using AppliComptaApi.src.Models;
using AppliComptaApi.src.Models.Wrappers;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppliComptaApi.src.Controllers
{
    [Authorize]
    [Route("api/accountingDocuments")]
    [ApiController]
    public class AccountingDocumentsController : ControllerBase
    {
        private readonly AppliContext _context;

        public AccountingDocumentsController(AppliContext context)
        {
            _context = context;
        }


        
        [HttpGet]
        public async Task<ActionResult<PagedResponse<IEnumerable<AccountingDocumentDTO>>>> GetAccountingDocuments([FromQuery] int pageNumber , [FromQuery] int pageSize, [FromQuery]  DocumentType? type)
        {
            IQueryable<AccountingDocument> accountingDocuments = _context.AccountingDocuments;

            if (type != null)
            {
                accountingDocuments = accountingDocuments.Where(accountingDocument => accountingDocument.Type == type);
            }

            var totalRecords = accountingDocuments.Count();

            var pagedData = accountingDocuments
                .Skip((pageNumber -1) * pageSize)
                .Take(pageSize)
                .Select(accountingDocument => new AccountingDocument

                {

                    Id = accountingDocument.Id,
                    Number = accountingDocument.Number,
                    Type = accountingDocument.Type,
                    Date = accountingDocument.Date,
                    FeeDesignation = accountingDocument.FeeDesignation,
                    FeeAmount = accountingDocument.FeeAmount,
                    CommercialNet = accountingDocument.CommercialNet,
                    VAT = accountingDocument.VAT,
                    NetPayable = accountingDocument.NetPayable,
                    CustomerId = accountingDocument.CustomerId

                })
                .ToList();



            return Ok(new PagedResponse<IEnumerable<AccountingDocument>>(pagedData, pageNumber, pageSize, totalRecords));
        }

        
        [HttpGet("xls")]

        public async Task<IActionResult> GetDownloadExcel(DocumentType? type)
        {
            using MemoryStream memoryStream = new ();
            using (
            SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook)
            )
            {
                WorkbookPart workbookpart = document.AddWorkbookPart();
                workbookpart.Workbook = new Workbook(); WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                SheetData sheetData = new();
                worksheetPart.Worksheet = new Worksheet(sheetData); Sheets sheets = document.WorkbookPart!.Workbook.
                AppendChild(new Sheets()); Sheet sheet = new()
                {
                    Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Sample Data"
                };
                Row rowHeader = new();
                rowHeader.Append(new Cell()
                {
                    CellValue = new CellValue("Number"),
                    DataType = CellValues.String
                });
                rowHeader.Append(new Cell()
                {
                    CellValue = new CellValue("Type"),
                    DataType = CellValues.String
                });
                rowHeader.Append(new Cell()
                {
                    CellValue = new CellValue("Date"),
                    DataType = CellValues.String
                });
                rowHeader.Append(new Cell()
                {
                    CellValue = new CellValue("FeeDesignation"),
                    DataType = CellValues.String
                });
                rowHeader.Append(new Cell()
                {
                    CellValue = new CellValue("FeeAmount"),
                    DataType = CellValues.String
                });
                rowHeader.Append(new Cell()
                {
                    CellValue = new CellValue("CommercialNet"),
                    DataType = CellValues.String
                });
                rowHeader.Append(new Cell()
                {
                    CellValue = new CellValue("VAT"),
                    DataType = CellValues.String
                });
                rowHeader.Append(new Cell()
                {
                    CellValue = new CellValue("NetPayable"),
                    DataType = CellValues.String
                });
                rowHeader.Append(new Cell()
                {
                    CellValue = new CellValue("CustomerId"),
                    DataType = CellValues.String
                });

                sheetData.Append(rowHeader);

                var accountingDocuments = _context.AccountingDocuments.AsQueryable();

                        if (type != null)
                {
                accountingDocuments = _context.AccountingDocuments.Where(accountingDocument => accountingDocument.Type == type.Value)
                    ;
                }

                foreach (AccountingDocument accountingDocument in accountingDocuments)
                {

                    Row row = new();
                    row.Append(new Cell()
                    {
                        CellValue = new CellValue(accountingDocument.Number),
                        DataType = CellValues.String
                    });
                    row.Append(new Cell()
                    {
                        CellValue = new CellValue(accountingDocument.Type.ToString()),
                        DataType = CellValues.String
                    });
                    row.Append(new Cell()
                    {
                        CellValue = new CellValue(accountingDocument.Date),
                        DataType = CellValues.String
                    });
                    row.Append(new Cell()
                     {
                        CellValue = new CellValue(accountingDocument.FeeDesignation),
                        DataType = CellValues.String
                    });
                    row.Append(new Cell()
                     {
                        CellValue = new CellValue(accountingDocument.FeeAmount.ToString()),
                        DataType = CellValues.String
                    });
                    row.Append(new Cell()
                     {
                        CellValue = new CellValue(accountingDocument.CommercialNet.ToString()),
                        DataType = CellValues.String
                    });
                    row.Append(new Cell()
                     {
                        CellValue = new CellValue(accountingDocument.VAT.ToString()),
                        DataType = CellValues.String
                    });
                    row.Append(new Cell()
                    {
                        CellValue = new CellValue(accountingDocument.NetPayable.ToString()),
                        DataType = CellValues.String
                    });
                    row.Append(new Cell()
                    {
                        CellValue = new CellValue(accountingDocument.CustomerId.ToString()),
                        DataType = CellValues.String
                    });

                    sheetData.Append(row);
                    }
                    sheets.Append(sheet);
                    workbookpart.Workbook.Save();
                }

            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SampleData.xlsx");
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountingDocumentDTO>> GetAccountingDocument(int id)
        {

            var accountingDocument = await _context.AccountingDocuments.FindAsync(id);

            if (accountingDocument == null)
            {
                return NotFound();
            }

            var accountingDocumentDTO = new AccountingDocumentDTO
            {
                Id = accountingDocument.Id,
                Number = accountingDocument.Number,
                Type = accountingDocument.Type,
                Date = accountingDocument.Date,
                FeeDesignation = accountingDocument.FeeDesignation,
                FeeAmount = accountingDocument.FeeAmount,
                CommercialNet = accountingDocument.CommercialNet,
                VAT = accountingDocument.VAT,
                NetPayable = accountingDocument.NetPayable,
                CustomerId = accountingDocument.CustomerId
            };

            return accountingDocumentDTO;
        }

        private bool validSpecialRules(AccountingDocumentDTO accountingDocument)
        {
            if (accountingDocument.FeeAmount == accountingDocument.CommercialNet
                && accountingDocument.VAT == accountingDocument.CommercialNet * 0.20
                && accountingDocument.NetPayable == accountingDocument.CommercialNet + accountingDocument.VAT)
            {
                return true;
            }

            return false;

        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountingDocument(int id, AccountingDocumentDTO accountingDocumentDTO)
        {
            if (id != accountingDocumentDTO.Id)
            {
                return BadRequest();
            }

            var accountingDocument = await _context.AccountingDocuments.FindAsync(id);
            if (accountingDocument == null)
            {
                return NotFound();
            }

            accountingDocument.Number = accountingDocumentDTO.Number;
            accountingDocument.Type = (DocumentType)accountingDocumentDTO.Type;
            accountingDocument.Date = accountingDocumentDTO.Date;
            accountingDocument.FeeDesignation = accountingDocumentDTO.FeeDesignation;
            accountingDocument.FeeAmount = accountingDocumentDTO.FeeAmount;
            accountingDocument.CommercialNet = accountingDocumentDTO.CommercialNet;
            accountingDocument.VAT = accountingDocumentDTO.VAT;
            accountingDocument.NetPayable = accountingDocumentDTO.NetPayable;
            accountingDocument.CustomerId = accountingDocumentDTO.CustomerId;

            if (!validSpecialRules(accountingDocumentDTO))
            {
                throw new Exception("invalid_special_rules");
            }

                await _context.SaveChangesAsync();

                return NoContent();
        }

        
        [HttpPost]
        public async Task<ActionResult<AccountingDocument>> PostAccountingDocument(AccountingDocumentDTO accountingDocumentDTO)
        {
            var accountingDocument = new AccountingDocument
            {
                Number = accountingDocumentDTO.Number,
                Type = (DocumentType)accountingDocumentDTO.Type,
                Date = accountingDocumentDTO.Date,
                FeeDesignation = accountingDocumentDTO.FeeDesignation,
                FeeAmount = accountingDocumentDTO.FeeAmount,
                CommercialNet = accountingDocumentDTO.CommercialNet,
                VAT = accountingDocumentDTO.VAT,
                NetPayable = accountingDocumentDTO.NetPayable,
                CustomerId = accountingDocumentDTO.CustomerId,

            };

            if (!validSpecialRules(accountingDocumentDTO))
            {
                throw new Exception("invalid_special_rules");
            }

            _context.AccountingDocuments.Add(accountingDocument);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAccountingDocument),
                new { id = accountingDocument.Id },
                accountingDocument);
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccountingDocument(int id)
        {
            var accountingDocument = await _context.AccountingDocuments.FindAsync(id);

            if (accountingDocument == null)
            {
                return NotFound();
            }

            _context.AccountingDocuments.Remove(accountingDocument);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
    }
}
