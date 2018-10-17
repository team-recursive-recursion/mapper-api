(SELECT l."UserID", FIRST(l."PointRaw"), MAX(l."CreatedAt") AS CreatedAt From public."LiveLocation" l Where ST_Contains(	
	(SELECT ST_Buffer(
					(SELECT (ST_Centroid(ST_Union(ST_GeomFromWKB(ie."Raw")))) FROM public."Elements" ie
					where ie."ZoneID" = 'a92a2974-99db-4f73-a6c5-1f509452170b'), --Point

					(SELECT max(ST_DistanceSphere(
							ST_geomFromWKB(e."Raw"), 
							(SELECT (ST_Centroid(ST_Union(ST_GeomFromWKB(ie."Raw")))) FROM public."Elements" ie
							where ie."ZoneID" = 'a92a2974-99db-4f73-a6c5-1f509452170b')
						))
						FROM public."Elements" e 
						WHERE e."ZoneID" = 'a92a2974-99db-4f73-a6c5-1f509452170b')/1000, -- Radius
						'quad_segs=8' --Form
		)),
		ST_GeomFromWKB(l."PointRaw")
	) AND (EXTRACT(MINUTE FROM  (now() - l."CreatedAt")) < 10) -- Limits the result to only the last 10 minutes
 	GROUP BY l."UserID"
 	ORDER BY CreatedAt 
)

-- 'a92a2974-99db-4f73-a6c5-1f509452170b' THIS IS A COURSE ID

-- -- Create a function that always returns the first non-NULL item
-- CREATE OR REPLACE FUNCTION public.first_agg ( anyelement, anyelement )
-- RETURNS anyelement LANGUAGE SQL IMMUTABLE STRICT AS $$
--         SELECT $1;
-- $$;
 
-- -- And then wrap an aggregate around it
-- CREATE AGGREGATE public.FIRST (
--         sfunc    = public.first_agg,
--         basetype = anyelement,
--         stype    = anyelement
-- );