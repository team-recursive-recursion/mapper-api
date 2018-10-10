(SELECT l."UserID", FIRST(l."PointRaw"), MAX(l."CreatedAt") AS CreatedAt From public."LiveLocation" l Where ST_Contains(	
	(SELECT ST_Buffer(
					(SELECT (ST_Centroid(ST_Union(ST_GeomFromWKB(ie."PointRaw")))) FROM public."Elements" ie
					where ie."CourseId" = '7c760b21-2cd4-4eaa-b50c-682b8228ec1f'), --Point

					(SELECT max(ST_DistanceSphere(
							ST_geomFromWKB(e."PolygonRaw"), 
							(SELECT (ST_Centroid(ST_Union(ST_GeomFromWKB(ie."PointRaw")))) FROM public."Elements" ie
							where ie."CourseId" = '7c760b21-2cd4-4eaa-b50c-682b8228ec1f')
						))
						FROM public."Elements" e 
						WHERE e."CourseId" = '7c760b21-2cd4-4eaa-b50c-682b8228ec1f')/1000, -- Radius
						'quad_segs=8' --Form
		)),
		ST_GeomFromWKB(l."PointRaw")
	) AND (EXTRACT(MINUTE FROM  (now() - l."CreatedAt")) < 10) -- Limits the result to only the last 10 minutes
 	GROUP BY l."UserID"
 	ORDER BY CreatedAt 
)

-- '7c760b21-2cd4-4eaa-b50c-682b8228ec1f' THIS IS A COURSE ID

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