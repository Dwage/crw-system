CREATE OR REPLACE FUNCTION check_salary_minimum()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW."Position" = 'Должность 1' AND NEW."Salary" < 20000 THEN
        RAISE EXCEPTION 'Зарплата сотрудника ниже минимально допустимой для Должности 1 (20,000)';
    ELSIF NEW."Position" = 'Должность 2' AND NEW."Salary" < 25000 THEN
        RAISE EXCEPTION 'Зарплата сотрудника ниже минимально допустимой для Должности 2 (30,000)';
    ELSIF NEW."Position" = 'Должность 3' AND NEW."Salary" < 30000 THEN
        RAISE EXCEPTION 'Зарплата сотрудника ниже минимально допустимой для Должности 3 (40,000)';
    ELSIF NEW."Position" = 'Должность 4' AND NEW."Salary" < 35000 THEN
        RAISE EXCEPTION 'Зарплата сотрудника ниже минимально допустимой для Должности 4 (50,000)';
    ELSIF NEW."Position" = 'Должность 5' AND NEW."Salary" < 40000 THEN
        RAISE EXCEPTION 'Зарплата сотрудника ниже минимально допустимой для Должности 5 (60,000)';
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_check_salary_minimum
BEFORE INSERT OR UPDATE ON "Staff"
FOR EACH ROW
EXECUTE FUNCTION check_salary_minimum();

CREATE OR REPLACE FUNCTION prevent_absolute_duplicates()
RETURNS TRIGGER AS $$
DECLARE
    duplicate_count INTEGER;
BEGIN
    SELECT COUNT(*)
    INTO duplicate_count
    FROM "CarRepairs"
    WHERE "WorkshopId" = NEW."WorkshopId"
      AND "CarId" = NEW."CarId"
      AND "MalfunctionId" = NEW."MalfunctionId"
      AND "TeamId" = NEW."TeamId"
      AND "StartDate" = NEW."StartDate"
      AND "EndDate" = NEW."EndDate";

    IF duplicate_count > 0 THEN
        RAISE EXCEPTION 'Запись с такими же значениями уже существует в таблице CarRepairs.';
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_prevent_absolute_duplicates
BEFORE INSERT ON "CarRepairs"
FOR EACH ROW
EXECUTE FUNCTION prevent_absolute_duplicates();

CREATE OR REPLACE FUNCTION validate_end_date_after_start_date()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW."EndDate" IS NOT NULL AND NEW."EndDate" < NEW."StartDate" THEN
        RAISE EXCEPTION 'Дата окончания должна быть позже даты начала для ремонта %', NEW."RepairId";
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_validate_end_date_after_start_date
BEFORE INSERT OR UPDATE ON "CarRepairs"
FOR EACH ROW
EXECUTE FUNCTION validate_end_date_after_start_date();

CREATE VIEW "LaborPerformance" AS  
WITH labor_performance AS (
         SELECT t."TeamId",
            t."Name" AS "TeamName",
            cr."RepairId",
            cr."CarId",
            cr."StartDate",
            cr."EndDate",
            EXTRACT(epoch FROM (((cr."EndDate" - cr."StartDate") / (3600)::double precision) / (24)::double precision)) AS "DaysSpent"
           FROM ("Teams" t
             JOIN "CarRepairs" cr ON ((cr."TeamId" = t."TeamId")))
        )
 SELECT "TeamId",
    "TeamName",
    count("RepairId") AS "RepairsCount",
    sum("DaysSpent") AS "TotalDaysSpent",
    avg("DaysSpent") AS "AvgDaysPerOrder"
   FROM labor_performance lp
  GROUP BY "TeamId", "TeamName"
  ORDER BY "TeamId";;

CREATE VIEW "MalfunctionFrequency" AS  
WITH malfunction_frequency AS (
         SELECT m."Name" AS "MalfunctionName",
            cm."Brand",
            cm."Model",
            count(cr."RepairId") AS "RepairCount"
           FROM ((("CarRepairs" cr
             JOIN "Malfunctions" m ON ((cr."MalfunctionId" = m."MalfunctionId")))
             JOIN "Cars" c ON ((cr."CarId" = c."CarId")))
             JOIN "CarModels" cm ON ((c."ModelId" = cm."ModelId")))
          GROUP BY m."Name", cm."Brand", cm."Model"
        )
 SELECT "MalfunctionName",
    "Brand",
    "Model",
    sum("RepairCount") AS "TotalRepairs"
   FROM malfunction_frequency
  GROUP BY "MalfunctionName", "Brand", "Model"
  ORDER BY "MalfunctionName" DESC;

CREATE VIEW "RepairCosts" AS  
WITH costforonemalfunction AS (
         SELECT c."CarId",
            c."Owner",
            cr."MalfunctionId",
            cr."RepairId",
            sp."PartId",
            sp."Quantity",
            cr."StartDate",
            cr."EndDate",
            ((sp."Price" * (sp."Quantity")::numeric) + m."LaborCost") AS "TotalPrice"
           FROM (((("Cars" c
             JOIN "CarRepairs" cr ON ((c."CarId" = cr."CarId")))
             JOIN "CarModels" cm ON ((cm."ModelId" = c."ModelId")))
             JOIN "Malfunctions" m ON ((m."MalfunctionId" = cr."MalfunctionId")))
             JOIN "SpareParts" sp ON (((m."MalfunctionId" = sp."MalfunctionId") AND (cm."ModelId" = sp."ModelId"))))
        )
 SELECT "Owner",
    "CarId",
    "StartDate",
    "EndDate",
    sum("TotalPrice") AS "TotalPrice",
    array_agg("MalfunctionId") AS "MalfunctionIds"
   FROM costforonemalfunction
  GROUP BY "Owner", "CarId", "StartDate", "EndDate";;
