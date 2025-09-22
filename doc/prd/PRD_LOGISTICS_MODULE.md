# Project Requirement Document (PRD) - Logisztikai Modul

## Áttekintés
Ez a dokumentum meghatározza a követelményeket egy modern, reszponzív logisztikai modul fejlesztésére, amely mikroszerviz architektúrára épül és Kubernetes-kompatibilis környezetben fut. A modul célja a raktári funkciók, csomagküldés, számlázás, valamint be- és kivételezés kezelése. A fejlesztés során kiemelt hangsúlyt kell fektetni a karbantarthatóságra, tesztelhetőségre és CI/CD folyamatokra, összhangban a CODING_REQUIREMENTS.md dokumentumban leírtakkal.

### Főbb technológiák
- **Frontend**: 
  - TypeScript, Angular 14+
  - NgRx állapotkezelés
  - i18next lokalizáció
  - OWASP biztonsági irányelvek
  
- **Backend**: 
  - .NET 6.0+ (C# 10+)
  - Entity Framework Core 7.0+
  - Dapper nagy teljesítményű lekérdezésekhez
  - MediatR CQRS implementációhoz
  - FluentValidation
  
- **Adatbázis**: 
  - MS SQL Server 2022 (elsődleges)
  - Támogatott alternatívák: PostgreSQL, MySQL
  - TDE (Transparent Data Encryption)
  - Always Encrypted bizalmas adatokhoz
  
- **Üzenetküldés és Stream feldolgozás**:
  - Apache Kafka üzenetsorokhoz
  - Apache Flink valós idejű feldolgozáshoz
  - TLS 1.2+ titkosítás a kommunikációhoz
  
- **Konténerizáció és Orchestráció**:
  - Docker konténerek
  - Kubernetes fürtök
  - Helm chartok telepítéshez
  
- **Monitorozás és Naplózás**:
  - Prometheus + Grafana metrikákhoz
  - ELK Stack (Elasticsearch, Logstash, Kibana) naplókhoz
  - Azure Application Insights / AWS CloudWatch integráció
  
- **Biztonság**:
  - Azure Key Vault / AWS KMS a titkok kezeléséhez
  - JWT token alapú hitelesítés
  - OAuth 2.0 és OpenID Connect
  - Titkosítás minden érzékeny adathoz
  
- **CI/CD**:
  - **Elsődleges platform**: Azure DevOps
  - **Automatizált környezet kialakítás**:
    - Azure Resource Manager (ARM) sablonok
    - Terraform Infrastructure as Code (IaC)
    - Azure DevOps környezetek és deployment groups
    - Automatizált tesztkörnyezet kiépítése és lebontása
  - **Folyamatos Integráció (CI)**:
    - Automatikus build és egységtesztek minden kódbázis változtatásnál
    - Kódminőség ellenőrzés (SonarQube)
    - Biztonsági szkennelések (OWASP ZAP, WhiteSource)
    - Csomagfüggőségek ellenőrzése (npm audit, NuGet security check)
  - **Folyamatos Szállítás/Telepítés (CD)**:
    - Automatikus deployment különböző környezetekbe (dev, test, staging, production)
    - Blue-Green vagy Canary deployment stratégiák
    - Automatikus visszaállítás hibás verziók esetén
    - Jóváhagyási munkafolyamatok éles környezetbe történő telepítéshez
  - **Monitorozás és Jelentés**:
    - Build és release pipeline-ok állapotának nyomon követése
    - Tesztlefedettség és minőségmérőszámok
    - Üzemeltetési metrikák és riasztások
    - Biztonsági és megfelelőségi jelentések

### Architekturális elvek
- **Mikroszolgáltatás architektúra** különálló, skálázható szolgáltatásokkal
- **Több-bérlős (Multi-tenant) tervezés** adatszigorló elkülönítéssel
- **Event-driven design** aszinkron kommunikációval
- **Domain-Driven Design (DDD)** a komplex üzleti logika modellezéséhez
- **CQRS és Event Sourcing** kiválasztott modulokban
- **SaaS készenlét** bérlői önkiszolgáló felülettel
- **Titkosítás** minden érzékeny adathoz
- **Többnyelvűség (i18n) és akadálymentesítés (a11y)** teljes körű támogatása
- **CI/CD folyamatok** Azure DevOps-alapú teljes körű automatizálással, teszteléssel és biztonsági ellenőrzésekkel

A projekt kezdeti fázisa egy egyszerű üdvözlő oldal létrehozása, amely bemutatja a modult. A további funkcionalitásokat (raktári funkciók, csomagküldés, számlázás, be-/kivételezés) a későbbiekben fogjuk megadni és implementálni.

## 1. Projekt Célok és Hatáskör
- **Cél**: Egy teljes logisztikai modul fejlesztése, amely segíti a vállalatok raktári műveleteit, csomagküldését, számlázását és anyagmozgatását.
- **Hatáskör**:
  - Üdvözlő oldal: Egyszerű, reszponzív oldal, amely üdvözli a felhasználót és röviden bemutatja a modult.
  - Jövőbeli Funkcionalitások (későbbiekben bővítendők):
    - Raktári funkciók: Készletkezelés, termékek nyomonkövetése.
    - Csomagküldés: Csomagok létrehozása, nyomonkövetése, szállítási címkék generálása.
    - Számlázás: Automatizált számlák generálása és kezelése.
    - Be-/Kivételezés: Termékek be- és kivételezésének rögzítése, raktárkészlet frissítése.
- **Felhasználók**: Logisztikai dolgozók, raktárvezetők, adminisztrátorok.

## 2. Rendszerarchitektúra

### 2.1 Több-bérlős (Multi-tenant) Tervezés
- **Bérlői Modell**: Minden bérlő saját adattérrel rendelkezik, fizikailag vagy logikailag elkülönítve
- **Adatelkülönítés**: 
  - Adatbázis szintű elválasztás (database/schema per tenant)
  - Oszlop szintű bérlő azonosítás (TenantId)
  - Automatikus szűrés minden adatbázis lekérdezésnél
- **Bérlői Kontextus**: Minden kérés esetén automatikus bérlő-azonosítás (JWT claim, fejléc, aldomain)
- **Teljesítmény**: Bérlői adatok gyorsítótárazása Redis vagy memcached segítségével

### 2.2 Biztonsági Architektúra
- **Titkosítási Rendszer**:
  - Adatok titkosítva nyugalmi állapotban (AES-256)
  - Biztonságos kommunikáció (TLS 1.2+)
  - Oszlopszintű titkosítás érzékeny adatokhoz
  - Kulcskezelés Azure Key Vault / AWS KMS segítségével
- **Biztonsági Szabályzatok**:
  - Legkisebb jogosultság elve
  - Többtényezős hitelesítés
  - Rendszeres biztonsági auditok
  - Sebezhetőségi vizsgálatok

### 2.3 Mikroszolgáltatás Struktúra

A rendszer a következő főbb szolgáltatásokból fog állni:

1. **API Gateway** (Ocelot/YARP)
   - Bejövő kérések útválasztása
   - Hitelesítés és engedélyezés (JWT, OAuth 2.0)
   - Terheléselosztás és gyorsítótárazás
   - Bérlő-azonosítás és -érvényesítés
   - Kérések naplózása és monitorozása
   - Rátakorlátozás (Rate Limiting) bérlőnként

2. **Bérlői Szolgáltatás**
   - Bérlők életciklus kezelése (létrehozás, módosítás, törlés)
   - Számlázási és előfizetési adatok kezelése
   - Használati metrikák gyűjtése és riasztások

3. **Felhasználói Felület** (Angular)
   - Reszponzív, mobilbarát kialakítás
   - Többnyelvű támogatás (i18n)
   - Akadálymentesített felhasználói élmény (WCAG 2.1 AA)
   - Bérlői beállítások kezelése
   - Személyre szabható irányítópultok
   - Reszponzív webes felület
   - Progresszív Web App (PWA) támogatás
   - Offline működés támogatása

3. **Logisztikai Alapszolgáltatás** (.NET Core)
   - Alapvető logisztikai műveletek
   - Adatintegritás biztosítása
   - Tranzakciókezelés

4. **Raktárkezelő Szolgáltatás** (.NET Core)
   - Készletnyilvántartás
   - Raktárhelyek kezelése
   - Készletmozgások nyomon követése

5. **Szállítási Szolgáltatás** (.NET Core)
   - Csomagküldés kezelése
   - Szállítási címkék generálása
   - Szállítási státusz követése

6. **Számlázási Szolgáltatás** (.NET Core)
   - Számlák generálása
   - Fizetések kezelése
   - Kimutatások készítése

### 2.1 Képernyő-öröklődési modell

A rendszer egy közös ős-képernyőből (BaseScreen) származtatott képernyőket használ, mindkét rétegben (frontend és backend) egységesen.

**Központi követelmények:**
- Minden képernyő típusnak a BaseScreen-ből kell származnia
- A BaseScreen tartalmazza az összes alappéldányt és viselkedést
- A módosítások a BaseScreen-ben automatikusan érvényesülnek az összes leszármazott képernyőn
- Különleges esetekben lehessen felülírni az örökölt viselkedést

**BaseScreen főbb komponensei:**
- Fejléc (fejlesztői környezetben látható, élesben elrejtett)
- Navigációs menü
- Lábléc
- Betöltési állapot kezelése
- Hibaüzenetek kezelése
- Nyelvi váltás

### 2.2 Frontend implementáció

A frontend Angular keretrendszerre épül, és a következő tervezési mintákat követi:

#### 2.2.1 Alapvető szerkezet

```typescript
// Példa a BaseScreen osztályra (Angular komponens)
@Component({
  template: `
    <div class="base-screen">
      <app-header [title]="title" [loading]="loading"></app-header>
      
      <main>
        <ng-container *ngIf="loading">
          <app-loading-spinner></app-loading-spinner>
        </ng-container>
        
        <app-error-message *ngIf="error" [message]="error"></app-error-message>
        
        <div *ngIf="!loading && !error" class="content">
          <ng-content></ng-content>
        </div>
      </main>
      
      <app-footer></app-footer>
    </div>
  `,
  styleUrls: ['./base-screen.component.scss']
})
export abstract class BaseScreenComponent implements OnInit, OnDestroy {
  @Input() title: string = '';
  loading: boolean = false;
  error: string | null = null;
  
  // Dependency injection a szükséges szolgáltatásokhoz
  constructor(
    protected translate: TranslateService,
    protected errorHandler: ErrorHandlerService,
    protected loadingService: LoadingService
  ) {}
  
  // Életciklus metódusok
  ngOnInit(): void {
    this.initialize();
  }
  
  ngOnDestroy(): void {
    this.cleanup();
  }
  
  // Kötelezően implementálandó metódusok
  protected abstract initialize(): void;
  protected abstract cleanup(): void;
  
  // Közös metódusok
  protected setLoading(state: boolean): void {
    this.loading = state;
    this.loadingService.setLoading(state);
  }
  
  protected handleError(error: any): void {
    this.error = this.errorHandler.handleError(error);
    this.loading = false;
  }
  
  // Nyelvváltás kezelése
  protected changeLanguage(lang: string): void {
    this.translate.use(lang);
    localStorage.setItem('userLanguage', lang);
  }
}
```

#### 2.2.2 State Management (NgRx)

```typescript
// Állapotkezelés NgRx-el
@Injectable()
export class ScreenEffects {
  constructor(
    private actions$: Actions,
    private screenService: ScreenService
  ) {}

  loadScreenData$ = createEffect(() => 
    this.actions$.pipe(
      ofType(ScreenActions.loadScreenData),
      switchMap(({ screenId }) => 
        this.screenService.getScreenData(screenId).pipe(
          map(data => ScreenActions.loadScreenDataSuccess({ data })),
          catchError(error => of(ScreenActions.loadScreenDataFailure({ error })))
        )
      )
    )
  );
}
```
```

### 2.3 Backend implementáció (.NET Core)

#### 2.3.1 Alapvető szerkezet

```csharp
// Alap modell a képernyőkhöz
public abstract class BaseScreenModel
{
    [JsonIgnore]
    public string RequestId { get; set; } = Guid.NewGuid().ToString();
    
    [JsonIgnore]
    public bool IsLoading { get; protected set; }
    
    [JsonIgnore]
    public string ErrorMessage { get; protected set; }
    
    public string CurrentLanguage { get; set; } = "hu";
    
    [JsonIgnore]
    public Dictionary<string, string> ValidationErrors { get; } = new();
    
    protected virtual void Initialize()
    {
        this.IsLoading = false;
        this.ErrorMessage = string.Empty;
    }
    
    protected void SetError(string message, Exception ex = null)
    {
        this.ErrorMessage = message;
        this.IsLoading = false;
        // Logolás stb.
    }
    
    public virtual void ClearValidation()
    {
        this.ValidationErrors.Clear();
    }
}

// Alap kontroller osztály
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public abstract class BaseApiController : ControllerBase
{
    protected readonly ILogger _logger;
    protected readonly IMediator _mediator;
    
    protected BaseApiController(ILogger logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    protected async Task<ActionResult<T>> HandleQuery<T>(IRequest<T> query)
    {
        try
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error occurred");
            return BadRequest(ex.Errors);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing your request");
            return StatusCode(500, "Internal server error");
        }
    }
}
```

#### 2.3.2 CQRS implementáció

```csharp
// Példa Command és CommandHandler implementációra
public class CreateOrderCommand : IRequest<OrderDto>
{
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    
    public CreateOrderCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        ILogger<CreateOrderCommandHandler> logger)
    {
        _context = context;
        _currentUserService = currentUserService;
        _logger = logger;
    }
    
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            OrderNumber = request.OrderNumber,
            OrderDate = request.OrderDate,
            Status = OrderStatus.Created,
            CreatedBy = _currentUserService.UserId,
            CreatedAt = DateTime.UtcNow
        };
        
        // Tovább feldolgozás...
        
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Order {OrderId} created successfully", order.Id);
        
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            OrderDate = order.OrderDate,
            Status = order.Status.ToString()
        };
    }
}
```
```

## 3. Funkcionalitás (Kezdeti Fázis)

### 3.1 Alapképernyő (BaseScreen)
- **Központi vezérlés**: Minden képernyő alapja
- **Kötelező elemek**: 
  - Fejléc (alkalmazás neve, nyelvválasztó)
  - Betöltési állapot indikátor
  - Hibaüzelek megjelenítése
  - Navigációs menü
  - Lábléc (verziószám, copyright)
- **Kötelező funkcionalitások**: 
  - Nyelvváltás kezelése
  - Betöltési állapot kezelése
  - Hibaüzenetek kezelése
  - Navigáció kezelése

### 3.2 Üdvözlő Képernyő (HomeScreen)
- **Öröklés**: BaseScreen-ből származik
- **Egyedi tartalom**:
  - Üdvözlő üzenet a modulról
  - Interaktív gomb mikroszerviz híváshoz
  - Dinamikus üdvözlő üzenet megjelenítése
- **Kötelező felülírások**:
  - renderContent() - az egyedi tartalom megjelenítése
  - onInit() - adatok betöltése indításkor

### 3.3 Képernytípusok és öröklődési modell

Minden új képernyő a BaseScreen-ből fog származni, és csak az egyedi funkcionalitást kell implementálnia. Az alábbi alaptípusok állnak rendelkezésre a gyakori felhasználói felületi minták megvalósításához:

#### 3.3.1 Szerkesztő Képernyő (EditScreen)
- **Cél**: Adatbeviteli űrlapok megjelenítése és kezelése
- **Öröklés**: BaseScreen → EditScreen
- **Főbb jellemzők**:
  - Űrlap mezők dinamikus betöltése és validálása
  - Mentés, visszaállítás, megszakítás gombok
  - Automatikus mentés tervezett időközönként (opcionális)
  - Mezőszintű validáció és hibaüzenetek
  - Függő mezők kezelése

```typescript
abstract class EditScreen<T> extends BaseScreen {
  protected formData: T;
  protected isNew: boolean = true;
  protected validationErrors: Record<keyof T, string>;
  
  abstract getDefaultValues(): T;
  abstract validateForm(): boolean;
  abstract saveData(): Promise<boolean>;
  
  protected initializeForm(): void {
    this.formData = this.getDefaultValues();
    this.validationErrors = {} as Record<keyof T, string>;
  }
  
  protected handleInputChange(field: keyof T, value: any): void {
    this.formData[field] = value;
    // Automatikus validáció a mező módosításakor
    this.validateField(field);
  }
}
```

#### 3.3.2 Adatnézeti Képernyő (GridViewScreen)
- **Cél**: Adatok táblázatos megjelenítése és kezelése
- **Öröklés**: BaseScreen → GridViewScreen
- **Főbb jellemzők**:
  - Oldaltördelés és rendezés
  - Oszlopszűrés és keresés
  - Soronkénti műveletek (szerkesztés, törlés)
  - Exportálás (CSV, Excel, PDF)
  - Változások nyomon követése

```typescript
import { BaseScreen } from './BaseScreen';

/**
 * Constants for grid configuration
 */
const GRID_DEFAULTS = {
  PAGE_SIZE: 20,
  DEFAULT_SORT_COLUMN: 'id',
  DEFAULT_SORT_DIRECTION: 'asc' as const
} as const;

/**
 * Enum for sort direction
 */
enum SortDirection {
  ASC = 'asc',
  DESC = 'desc'
}

/**
 * Text alignment options for grid columns
 */
enum TextAlignment {
  LEFT = 'left',
  CENTER = 'center',
  RIGHT = 'right',
  JUSTIFY = 'justify'
}

/**
 * Interface for grid column definitions
 */
interface GridColumnDef<T> {
  /** The field name from the data object */
  field: keyof T;
  /** Display name for the column header */
  headerName: string;
  /** Whether the column is sortable */
  sortable?: boolean;
  /** Whether the column is filterable */
  filterable?: boolean;
  /** Width of the column in pixels */
  width?: number;
  /** Text alignment within the column */
  align?: TextAlignment;
}

/**
 * Abstract base class for grid view screens
 */
abstract class GridViewScreen<T> extends BaseScreen {
  // Grid data and state
  protected data: T[] = [];
  protected currentPage: number = 1;
  protected pageSize: number = GRID_DEFAULTS.PAGE_SIZE;
  protected sortColumn: string = GRID_DEFAULTS.DEFAULT_SORT_COLUMN;
  protected sortDirection: SortDirection = GRID_DEFAULTS.DEFAULT_SORT_DIRECTION;
  protected filters: Record<string, any> = {};
  
  abstract loadData(): Promise<void>;
  abstract getColumns(): GridColumnDef<T>[];
  
  /**
   * Resets the grid to its initial state
   */
  protected resetGrid(): void {
    this.currentPage = 1;
    this.sortColumn = GRID_DEFAULTS.DEFAULT_SORT_COLUMN;
    this.sortDirection = GRID_DEFAULTS.DEFAULT_SORT_DIRECTION;
    this.filters = {};
    this.loadData();
  }
  
  /**
   * Applies filters and reloads data
   * @param newFilters - New filter values to apply
   */
  protected applyFilters(newFilters: Record<string, any>): void {
    this.filters = { ...this.filters, ...newFilters };
    this.currentPage = 1; // Reset to first page when filters change
    this.loadData();
  }
  
  /**
   * Toggles the sort direction
   * @returns The new sort direction
   */
  protected toggleSortDirection(): SortDirection {
    return this.sortDirection === SortDirection.ASC 
      ? SortDirection.DESC 
      : SortDirection.ASC;
  }
}
```

#### 3.3.3 Kereső Képernyő (SearchDialog)
- **Cél**: Adatok szűrése és kiválasztása egy felugró ablakban
- **Öröklés**: BaseScreen → SearchDialog
- **Főbb jellemzők**:
  - Többszörös kiválasztás támogatása
  - Speciális szűrési lehetőségek
  - Előnézet a kiválasztott elemekről
  - Visszatérési érték a szülő képernyőnek

```typescript
// Search dialog configuration constants
const SEARCH_DIALOG_DEFAULTS = {
  MULTI_SELECT: false,
  PAGE_SIZE: 10,
  MIN_SEARCH_LENGTH: 3
} as const;

/**
 * Search dialog component for selecting items from a list
 */
class SearchDialog<T> extends BaseScreen {
  private resolve: (value: T[] | null) => void;
  private selectedItems: T[] = [];
  
  constructor(
    private readonly title: string,
    private readonly columns: GridColumnDef<T>[],
    private readonly dataLoader: (filters: any) => Promise<T[]>,
    private readonly multiSelect: boolean = SEARCH_DIALOG_DEFAULTS.MULTI_SELECT,
    private readonly pageSize: number = SEARCH_DIALOG_DEFAULTS.PAGE_SIZE,
    private readonly minSearchLength: number = SEARCH_DIALOG_DEFAULTS.MIN_SEARCH_LENGTH
  ) {
    super();
  }
  
  public show(): Promise<T[] | null> {
    return new Promise((resolve) => {
      this.resolve = resolve;
      // Megjelenítés logika
    });
  }
  
  protected handleSelect(): void {
    this.resolve(this.selectedItems);
    this.close();
  }
  
  protected handleCancel(): void {
    this.resolve(null);
    this.close();
  }
}
```

#### 3.3.4 Speciális Képernytípusok

##### 3.3.4.1 Mester-Detail Képernyő (MasterDetailScreen)
- **Leírás**: Fő lista és a kiválasztott elem részleteinek együttes megjelenítése
- **Használati esetek**: Terméklista és termék részletek, Rendelés és tétellista

##### 3.3.4.2 Lépésről-lépésre Végrehajtás (WizardScreen)
- **Leírás**: Többlépéses folyamatok vezérlése
- **Használati esetek**: Új rendelés létrehozása, Regisztrációs folyamat

### 3.4 Jövőbeli üzleti képernyők tervezete
- Raktárkezelő Képernyő (WarehouseScreen) - GridViewScreen-ből származik
- Csomagküldő Képernyő (ShippingScreen) - WizardScreen-ből származik
- Számlázó Képernyő (BillingScreen) - MasterDetailScreen-ből származik
- Be-/Kivételezési Képernyő (InventoryScreen) - GridViewScreen-ből származik

## 4. Mikroszerviz Specifikáció (Üdvözlő Funkció)
- **Végpont**: `/api/greeting`
- **Metódus**: GET
- **Válasz formátuma**: JSON
  ```json
  {
    "message": "Üdvözöljük a Logisztikai Modulban!"
  }
  ```
- **Hibakezelés**: 
  - 200 OK: Sikeres kérés, visszaadja az üdvözlő üzenetet
  - 500 Internal Server Error: Szerverhiba esetén
  - 503 Service Unavailable: Ha a szolgáltatás nem elérhető
- **Időtúllépés**: 5 másodperc
- **Újrapróbálkozás**: 2 alkalommal történik automatikus újrapróbálkozás hiba esetén

## 5. Nem Funkcionalitás Követelmények
- **Teljesítmény**: Gyors betöltés, optimalizált API hívások.
- **Biztonság**: HTTPS, input validáció, SQL injection védelem.
- **Skálázhatóság**: Mikroszerviz architektúra, hogy könnyen bővíthető legyen a későbbi funkcionalitásokkal.
- **Kompatibilitás**: Modern böngészők, mobil eszközök.

## 6. Architektúra és Technológia
- **Mikroszerviz Alapú Felépítés**: Külön mikroszervizek a frontend (JS/HTML/CSS), backend (C# .NET), adatbázis (MS SQL Server 2022) és egyéb szolgáltatások (lokalizáció, akadálymentesítés) számára.
### 8. Technológiai Részletek

#### 8.1 Frontend (Angular)
- **Követelmények**:
  - TypeScript 4.8+
  - Angular 14+
  - RxJS 7.0+ reaktív programozáshoz
  - NgRx állapotkezelés komplex alkalmazásokhoz
  - i18next lokalizáció többnyelvű támogatáshoz
  - Angular Material komponensek konzisztens UI-hoz
  - OWASP biztonsági irányelvek betartása
  - Teljes körű teszttakarás (Jest, Cypress)

#### 8.2 Backend (.NET Core)
- **Követelmények**:
  - .NET 6.0+ (C# 10+)
  - Entity Framework Core 7.0+ ORM megoldásként
  - Dapper nagy teljesítményű lekérdezésekhez
  - MediatR a CQRS minta implementálásához
  - FluentValidation a bemeneti adatok validálásához
  - JWT token alapú hitelesítés
  - Automatizált API dokumentáció (Swagger/OpenAPI)
  - Teljes körű egység- és integrációs tesztek

#### 8.3 Adattárolás és Adatbázis
- **MS SQL Server 2022** (elsődleges):
  - TDE (Transparent Data Encryption) az adatok védelméhez
  - Always Encrypted bizalmas adatokhoz
  - Particionálás nagy adatmennyiségek kezeléséhez
  - Columnstore indexek analitikai lekérdezésekhez
- **Támogatott alternatívák**:
  - PostgreSQL 14+
  - MySQL 8.0+
- **Adatbázis migrációk**:
  - Entity Framework Core Code-First migrációk
  - Idempotens migrációs szkriptek
  - Verziókövetés és visszaállítási lehetőségek

#### 8.4 Többnyelvűség (i18n)
- **Frontend**:
  - i18next könyvtár
  - JSON alapú fordítói fájlok (hu.json, en.json, stb.)
  - Dinamikus nyelvváltás
  - Formátumok lokalizálása (dátum, pénznem, stb.)
- **Backend**:
  - .NET beépített IStringLocalizer
  - Resource fájlok (resx) nyelvenként
  - Kulturális beállítások kezelése
  - Aszinkron lokalizációs szolgáltatások

#### 8.5 Akadálymentesítés (a11y)
- **WCAG 2.1 AA szintű megfelelés**
- **Frontend implementáció**:
  - ARIA címkék és attribútumok
  - Billentyűzet navigáció teljes támogatása
  - Színkontraszt (minimum 4.5:1)
  - Reszponzív design mobil eszközökhöz
  - Képek alternatív szövegei
  - Fókusz kezelés és láthatóság
- **Backend támogatás**:
  - Strukturált adatok (JSON-LD)
  - Akadálymentes API válaszok
  - Validációs hibaüzenetek akadálymentes formában

#### 8.6 Biztonság
- **Adatvédelem**:
  - GDPR és egyéb szabályozások betartása
  - Adatminimalizálás
  - Jogosultságkezelés (RBAC)
  - Naplózás és audit trail
- **Titkosítás**:
  - TLS 1.2+ minden kommunikációhoz
  - Adatok titkosítva nyugalmi állapotban
  - Biztonságos jelszó tárolás (sózással, hasheléssel)
  - Biztonságos titkos kulcsok kezelése
- **Biztonsági tesztek**:
  - Automatizált sebezhetőségi vizsgálatok
  - Penetrációs tesztek
  - Biztonsági kód áttekintések

## 10. Konfigurációkezelés

### 10.1 Konfigurációs Elvek
- **Minden beállítás kód nélkül**: Minden környezetfüggő beállítást konfigurációs forrásból kell betölteni
- **Titkos adatok védelme**: Soha ne legyenek jelszavak, API kulcsok vagy egyéb titkos adatok a kódbázisban
- **Környezetfüggetlenség**: Ugyanaz a kód fusson minden környezetben, csak a konfiguráció változzon
- **Verziókövetés**: A nem titkos konfigurációk verziókövetés alatt legyenek
- **Értelmezőkód mentesség**: A konfigurációk ne tartalmazzanak futtatható kódot

### 10.2 Konfigurációs Források

#### 10.2.1 Frontend (Angular)
```typescript
// environment.ts (fejlesztés)
export const environment = {
  production: false,
  apiUrl: 'https://api.dev.example.com',
  features: {
    analytics: true,
    darkMode: true
  }
};

// environment.prod.ts (termelés)
export const environment = {
  production: true,
  apiUrl: 'https://api.prod.example.com',
  features: {
    analytics: true,
    darkMode: true
  }
};
```

#### 10.2.2 Backend (.NET)
```json
// appsettings.json (alapértelmezett)
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*",
  "FeatureFlags": {
    "EnableExperimentalFeatures": false,
    "MaintenanceMode": false
  }
}

// appsettings.Development.json (fejlesztés)
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=LogisticsDev;Trusted_Connection=True;"
  },
  "ExternalServices": {
    "EmailService": "https://email-service.dev.example.com"
  }
}
```

#### 10.2.3 Docker és Kubernetes
```yaml
# docker-compose.override.yml
version: '3.8'

services:
  webapi:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Server=db;Database=Logistics;User=sa;Password=YourStrong!Passw0rd
    env_file:
      - .env.local

# k8s/configmap.yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: app-config
data:
  appsettings.json: |-
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information"
        }
      },
      "FeatureFlags": {
        "EnableExperimentalFeatures": "false"
      }
    }
```

### 10.3 Titkos Adatok Kezelése
- **Titkos kulcsok tárolása**:
  - Azure Key Vault
  - AWS Secrets Manager
  - HashiCorp Vault
  - Kubernetes Secrets
- **Fejlesztői környezet**:
  - `.env.local` fájl (`.gitignore`-ban)
  - VS Code launch settings
  - Docker `.env` fájlok
- **Éles környezet**:
  - CI/CD pipeline-ból történő injektálás
  - Titkosítás a nyugalmi állapotban
  - Automatikus rotáció

### 10.4 Konfiguráció Életciklusa
1. **Fejlesztés**: Lokális konfigurációk a fejlesztői gépen
2. **Verziókövetés**: Nem titkos konfigurációk a kódbázisban
3. **Build**: Konfigurációk csomagolása az alkalmazással
4. **Telepítés**: Környezetspecifikus értékek behelyettesítése
5. **Futásidő**: Dinamikus konfiguráció frissítések
6. **Naplózás**: Konfigurációváltozások nyomon követése

### 10.5 Követelmények
- Minden alkalmazás komponens támogassa a külső konfiguráció betöltését
- Konfigurációs értékek validálása induláskor
- Konfigurációváltozások naplózása
- Titkos adatok soha ne kerüljenek verziókövetésbe
- Konfigurációs sablonok dokumentálása

## 11. Biztonságos Fejlesztési Eljárások
### 11.1 Adatvédelem és Titkosítás
- **Adatbesorolás és -kezelés**:
  - Minden adatot osztályozni kell bizalmassági szint szerint (nyilvános, belső, bizalmas, szigorúan bizalmas)
  - Adatminimalizálás: Csak a szükséges adatok gyűjtése és tárolása
  - Adattisztítás: Nem szükséges adatok rendszeres törlése
  - Adatmegőrzési szabályzatok meghatározása és betartatása
  - Adatszabályozási követelmények kezelése (pl. GDPR, CCPA)
  - Adattulajdonosok és felelősök meghatározása

- **Titkosítási Szabványok**:
  - **Adatok nyugalmi állapotban**: AES-256 titkosítás
  - Adatátvitelhez: TLS 1.2+ (1.3 ajánlott)
  - Nyilvános kulcsú titkosításhoz: RSA-2048 vagy erősebb, ECDSA-256
  - Hash függvények: SHA-256 vagy erősebb (SHA3-256, BLAKE3)
  - Jogkivonatok: JWT (RS256/ES256) vagy PASETO
  - Jelszavak: Argon2id vagy bcrypt (munkaigényes hash)
  - Digitális aláírások: ECDSA vagy EdDSA
  - Kulcscsere: ECDH vagy DH
  - Véletlenszám generálás: CSPRNG (Cryptographically Secure PRNG)

- **Adattitkosítási Stratégiák**:
  - **Adatbázis szintű titkosítás (TDE)**: Minden adatbázis esetén engedélyezve
  - Oszlopszintű titkosítás: Bizalmas adatokhoz (PII, pénzügyi adatok)
  - Always Encrypted: Az alkalmazás szintjén történő titkosítás
  - Titkosított fájlrendszer: Az alkalmazás által használt fájlrendszer szintjén
  - Ügyféloldali titkosítás: Bizalmas adatok esetén, mielőtt elhagynák az ügyféleszközt
  - Homomorf titkosítás: Speciális esetekben, ahol titkosított adatokon kell műveletet végezni
  - Zero-Knowledge Proof: Jelszavak és hitelesítő adatok ellenőrzése a tényleges értékek ismerete nélkül

- **Kulcskezelés és Titkosítási Életciklus**:
  - **Kulcstároló megoldások**:
    - Azure Key Vault
    - AWS Key Management Service (KMS)
    - HashiCorp Vault
    - Google Cloud Key Management
  - Kulcs életciklus kezelése:
    - Biztonságos kulcsgenerálás
    - Kulcs tárolása HSM-ben (Hardware Security Module)
    - Kulcsrotáció szabályzatok (90-365 nap)
    - Kulcselforgatás támogatása
    - Kulcstörlés és archiválás
    - Kulcshasználat naplózása
  - Több régiós kulcstárolás katasztrófa-helyreállításhoz
  - Kulcstároló magas rendelkezésre állása
  - Kulcshozzáférési szabályzatok és szerepköralapú hozzáférés-vezérlés (RBAC)

- **Titkosítási Megoldások Alkalmazás Rétegenként**:
  | Réteg | Titkosítási Megoldás | Megvalósítás |
  |--------|----------------------|--------------|
  | Felhasználói Felület | TLS 1.3, JWT token titkosítás | HTTPS, HttpOnly + Secure flag cookie-khoz |
  | API Réteg | mTLS, JWT aláírás | API Gateway konfiguráció |
  | Alkalmazás Réteg | In-memory titkosítás | Bizalmas adatok memóriában való kezelése |
  | Adatbázis Réteg | TDE, Always Encrypted | Adatbázis konfiguráció |
  | Tárolási Réteg | Tároló titkosítás | Azure Storage Service Encryption |
  | Biztonsági Másolatok | Titkosított biztonsági mentések | AES-256 titkosítás |
  | Hálózati Réteg | IPsec, WireGuard | VPN kapcsolatok |
  | Naplók | Titkosított naplózás | Naplófeldolgozási pipeline |

- **Titkosítási Követelmények Szolgáltatásonként**:
  - **Felhasználói Hitelesítés**:
    - Jelszavak: Argon2id hashelés + egyedi só
    - MFA tokenek: TOTP (Time-based One-Time Password)
    - Jogkivonatok: JWT (RS256) vagy PASETO
  - **Adatbázis**:
    - TDE (Transparent Data Encryption)
    - Always Encrypted a bizalmas oszlopokhoz
    - SSL/TLS az adatbázis kapcsolatokhoz
  - **Üzenetsor-kezelés**:
    - Üzenetek titkosítása a küldő oldalon
    - TLS az üzenetküldő szolgáltatások között
  - **Tárolás**:
    - Kiszolgálóoldali titkosítás (SSE)
    - Ügyféloldali titkosítás kritikus adatokhoz
  - **Hálózat**:
    - TLS 1.3 minden kommunikációhoz
    - mTLS szolgáltatások között
  - **Monitorozás és Naplózás**:
    - Titkosított naplók
    - Érzékeny adatok maszkolása

- **Titkosítási Kulcsok Kezelése CI/CD-ben**:
  - Titkos kulcsok soha ne legyenek verziókezelt kódbázisban
  - Biztonságos titkos változók kezelése Azure DevOps-ban
  - Automatikus kulcsrotáció a CI/CD folyamat részeként
  - Kulcsok életciklusának kezelése
  - Kulcshasználat naplózása és monitorozása
  - Vészhelyreállítási folyamatok a kulcsok elvesztése esetére

- **Fejlesztői Környezet Biztonsága**:
  - Fejlesztői gépek lemez titkosítása (BitLocker, FileVault)
  - Forráskód titkosítva a fejlesztői gépeken
  - IDE és fejlesztői eszközök biztonságos konfigurációja
  - Lokális adatbázisok és szolgáltatások titkosítása
  - Fejlesztői tanúsítványok és kulcsok biztonságos kezelése
