﻿/*
 * Copyright 2018, Richard Vasquez
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Original version written in C, Copyright 2016-2017 Uber Technologies, Inc.
 * C version licensed under the Apache License, Version 2.0 (the "License");
 * C Source code available at: https://github.com/uber/h3
 */
using System;
using System.Collections.Generic;

namespace H3Net.Code
{
    /// <summary>
    /// Linked data structure for geo data
    /// </summary>
    /// <!-- Based off 3.1.1 -->
    public class LinkedGeo
    {
        public class LinkedGeoCoord
        {
            public GeoCoord vertex;
            public  LinkedGeoCoord next;
        }
        public class LinkedGeoLoop
        {
            public  LinkedGeoCoord first;
            public  LinkedGeoCoord last;
            public LinkedGeoLoop next;
        }

        public class LinkedGeoPolygon
        {
            public  LinkedGeoLoop first;
            public  LinkedGeoLoop last;
            public LinkedGeoPolygon next;
        }

        public const int NORMALIZATION_SUCCESS = 0;
        public const int NORMALIZATION_ERR_MULTIPLE_POLYGONS = 1;
        public const int NORMALIZATION_ERR_UNASSIGNED_HOLES = 2;

        public LinkedGeo()
        {
        }

        internal static double  NORMALIZE_LON(double lon, bool isTransmeridian)
        {
            return isTransmeridian && lon < 0 ? lon + Constants.M_2PI : lon;
        }

        /// <summary>
        /// Take a given LinkedGeoLoop data structure and check if it
        /// contains a given geo coordinate.
        /// </summary>
        /// <param name="loop">The linked loop</param>
        /// <param name="bbox">The bbox for the loop</param>
        /// <param name="coord">The coordinate to check</param>
        /// <returns>Whether the point is contained</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool pointInsideLinkedGeoLoop(ref LinkedGeoLoop loop, ref  BBox bbox, ref GeoCoord coord)
        {
            // fail fast if we're outside the bounding box
            if (!BBox.bboxContains(bbox, coord))
            {
                return false;
            }
            bool isTransmeridian =BBox. bboxIsTransmeridian(bbox);
            bool contains = false;

            double lat = coord.lat;
            double lng = NORMALIZE_LON(coord.lon, isTransmeridian);

            GeoCoord a;
            GeoCoord b;
            LinkedGeoCoord currentCoord = null;
            LinkedGeoCoord nextCoord = null;


            while (true) {
                currentCoord = currentCoord == null ? loop.first : currentCoord.next;
                if (currentCoord == null)
                {
                    break;
                }
            
                a= currentCoord.vertex;
                nextCoord = currentCoord.next == null ? loop.first : currentCoord.next;
                b = nextCoord.vertex;

                // Ray casting algo requires the second point to always be higher
                // than the first, so swap if needed
                if (a.lat > b.lat) {
                    GeoCoord tmp = new GeoCoord( a.lat,a.lon);
                    a = new GeoCoord(b.lat, b.lon);
                    b = new GeoCoord(tmp.lat,tmp.lon);
                }

                // If we're totally above or below the latitude ranges, the test
                // ray cannot intersect the line segment, so let's move on
                if (lat < a.lat || lat > b.lat) {
                    continue;
                }

                double aLng = NORMALIZE_LON(a.lon, isTransmeridian);
                double bLng = NORMALIZE_LON(b.lon, isTransmeridian);

                // Rays are cast in the longitudinal direction, in case a point
                // exactly matches, to decide tiebreakers, bias westerly
                if (Math.Abs(aLng - lng) < Constants.EPSILON || Math.Abs(bLng - lng) < Constants.EPSILON) {
                    lng -= Constants.DBL_EPSILON;
                }

                // For the latitude of the point, compute the longitude of the
                // point that lies on the line segment defined by a and b
                // This is done by computing the percent above a the lat is,
                // and traversing the same percent in the longitudinal direction
                // of a to b
                double ratio = (lat - a.lat) / (b.lat - a.lat);
                double testLng =
                    NORMALIZE_LON(aLng + (bLng - aLng) * ratio, isTransmeridian);

                // Intersection of the ray
                if (testLng > lng) {
                    contains = !contains;
                }
            }

            return contains;
        }

        /// <summary>
        /// Create a bounding box from a simple polygon loop.
        /// Known limitations:
        /// - Does not support polygons with two adjacent points > 180 degrees of
        ///   longitude apart. These will be interpreted as crossing the antimeridian.
        /// - Does not currently support polygons containing a pole.
        /// </summary>
        /// <param name="loop">Loop of coordinates</param>
        /// <param name="bbox">bbox</param>
        /// <!-- Based off 3.1.1 -->
        public static void bboxFromLinkedGeoLoop(ref LinkedGeoLoop loop, ref BBox bbox)
        {
            // Early exit if there are no vertices
            if (loop.first == null)
            {
                bbox = new BBox();
                return;
            }

            bbox.south = Double.MaxValue;
            bbox.west = Double.MaxValue;
            bbox.north = -Double.MaxValue;
            bbox.east = -Double.MaxValue;
            double minPosLon = Double.MaxValue;
            double maxNegLon = -Double.MaxValue;
            bool isTransmeridian = false;

            double lat;
            double lon;
            GeoCoord coord;
            GeoCoord next;

            LinkedGeoCoord currentCoord = null;
            LinkedGeoCoord nextCoord = null;

            while (true) {

                currentCoord = currentCoord == null ? loop.first : currentCoord.next;
                if (currentCoord == null)
                {
                    break;
                }

                coord = currentCoord.vertex;
                nextCoord = currentCoord.next == null ? loop.first : currentCoord.next;
                next = nextCoord.vertex;


                lat = coord.lat;
                lon = coord.lon;
                if (lat < bbox.south) bbox.south = lat;
                if (lon < bbox.west) bbox.west = lon;
                if (lat > bbox.north) bbox.north = lat;
                if (lon > bbox.east) bbox.east = lon;
                // Save the min positive and max negative longitude for
                // use in the transmeridian case
                if (lon > 0 && lon < minPosLon){ minPosLon = lon;}
                if (lon < 0 && lon > maxNegLon){ maxNegLon = lon;}
                // check for arcs > 180 degrees longitude, flagging as transmeridian
                if (Math.Abs(lon - next.lon) >Constants. M_PI)
                {
                    isTransmeridian = true;
                }
            }
            // Swap east and west if transmeridian
            if (isTransmeridian) {
                bbox.east = maxNegLon;
                bbox.west = minPosLon;
            }
        }

        /// <summary>
        /// Whether the winding order of a given LinkedGeoLoop is clockwise
        /// </summary>
        /// <param name="loop">The loop to check</param>
        /// <returns>Whether the loop is clockwise</returns>
        /// <!-- Based off 3.1.1 -->
        static bool isClockwiseNormalizedLinkedGeoLoop(LinkedGeoLoop loop, bool isTransmeridian)
        {
            double sum = 0;
            GeoCoord a;
            GeoCoord b;

            LinkedGeoCoord currentCoord = null; 
            LinkedGeoCoord nextCoord = null;

            while (true)
            {
                currentCoord = currentCoord == null
                                   ? loop.first
                                   : currentCoord.next;
                if (currentCoord == null)
                {
                    break;
                }
            
                a= currentCoord.vertex;
                nextCoord = currentCoord.next == null
                                ? loop.first
                                : currentCoord.next;
                b = nextCoord.vertex;
                // If we identify a transmeridian arc (> 180 degrees longitude),
                // start over with the transmeridian flag set
                if (!isTransmeridian && Math.Abs(a.lon - b.lon) > Constants.M_PI) {
                    return isClockwiseNormalizedLinkedGeoLoop(loop, true);
                }
                sum += ((NORMALIZE_LON(b.lon, isTransmeridian) -
                         NORMALIZE_LON(a.lon, isTransmeridian)) *
                        (b.lat + a.lat));
            }

            return sum > 0;
        }

        /// <summary>
        /// Whether the winding order of a given loop is clockwise. In GeoJSON,
        /// clockwise loops are always inner loops (holes).
        /// </summary>
        /// <param name="loop">The loop to check</param>
        /// <returns>Whether the loop is clockwise</returns>
        /// <!-- Based off 3.1.1 -->
        public static bool isClockwiseLinkedGeoLoop(LinkedGeoLoop loop) {
            return isClockwiseNormalizedLinkedGeoLoop(loop, false);
        }

        /// <summary>
        /// Add a linked polygon to the current polygon
        /// </summary>
        /// <param name="polygon">Polygon to add link to</param>
        /// <returns>Pointer to new polygon</returns>
        /// <!-- Based off 3.1.1 -->
        public static LinkedGeoPolygon addNewLinkedPolygon(ref LinkedGeoPolygon polygon)
        {
            if (polygon.next != null)
            {
                throw new Exception("assert(polygon->next == NULL);");
            }
            
            LinkedGeoPolygon next = new LinkedGeoPolygon();

            polygon.next = next;
            return next;
        }

        /// <summary>
        /// Add a new linked loop to the current polygon
        /// </summary>
        /// <param name="polygon">Polygon to add loop to</param>
        /// <returns>Pointer to loop</returns>
        /// <!-- Based off 3.1.1 -->
        public static LinkedGeoLoop addNewLinkedLoop(ref LinkedGeoPolygon polygon)
        {
            LinkedGeoLoop loop = new LinkedGeoLoop();
            if (loop == null)
            {
                throw new Exception("FAIL: assert(loop != NULL)");
            }

            return addLinkedLoop(ref polygon,ref  loop);
        }

        /// <summary>
        /// Add an existing linked loop to the current polygon
        /// </summary>
        /// <param name="polygon">Polygon to add loop to</param>
        /// <returns>Pointer to loop</returns>
        /// <!-- Based off 3.1.1 -->
        public static  LinkedGeoLoop addLinkedLoop(ref LinkedGeoPolygon polygon, ref LinkedGeoLoop loop)
        {
            LinkedGeoLoop last = polygon.last;
            if (last == null) {
                if (polygon.first != null)
                {
                    throw new Exception("FAIL: assert(polygon->first == NULL)");
                }
                polygon.first = loop;
            } else {
                last.next = loop;
            }
            polygon.last = loop;
            return loop;
        }

        /// <summary>
        /// Add a new linked coordinate to the current loop
        /// </summary>
        /// <param name="loop">Loop to add coordinate to</param>
        /// <param name="vertex">Coordinate to add</param>
        /// <returns>Pointer to the coordinate</returns>
        /// <!-- Based off 3.1.1 -->
        public static LinkedGeoCoord addLinkedCoord(ref LinkedGeoLoop loop, ref GeoCoord vertex)
        {
            LinkedGeoCoord coord = new LinkedGeoCoord();
            coord.vertex = new GeoCoord(vertex.lat, vertex.lon);
            coord.next = null;

            LinkedGeoCoord last = loop.last;
            
            if (last == null) {
                if (loop.first != null)
                {
                    throw new Exception("assert(loop->first == NULL);");
                }
                loop.first = coord;
            } else {
                last.next = coord;
            }
            loop.last = coord;
            return coord;
        }

        /// <summary>
        /// Free all allocated memory for a linked geo loop. The caller is
        /// responsible for freeing memory allocated to input loop struct.
        /// </summary>
        /// <param name="loop">Loop to free</param>
        /// <!-- Based off 3.1.1 -->
        public static void destroyLinkedGeoLoop(ref LinkedGeoLoop loop)
        {
            LinkedGeoCoord nextCoord;
            for (LinkedGeoCoord currentCoord = loop.first; currentCoord != null;
                currentCoord = nextCoord)
            {
                nextCoord = currentCoord.next;
                // ReSharper disable once RedundantAssignment
                currentCoord = null;
            }
        }

        /// <summary>
        /// Free all allocated memory for a linked geo structure. The caller is
        /// responsible for freeing memory allocated to input polygon struct.
        /// </summary>
        /// <param name="polygon">Pointer to the first polygon in the structure</param>
        /// <!-- Based off 3.1.1 -->
        public static void destroyLinkedPolygon(ref LinkedGeoPolygon polygon)
        {
            // flag to skip the input polygon
            bool skip = true;
            LinkedGeoPolygon nextPolygon;
            LinkedGeoLoop nextLoop;
            for (LinkedGeoPolygon currentPolygon = polygon; currentPolygon !=null;
                currentPolygon = nextPolygon)
            {
                for (LinkedGeoLoop currentLoop = currentPolygon.first;
                    currentLoop != null; currentLoop = nextLoop)
                {
                    destroyLinkedGeoLoop(ref currentLoop);
                    nextLoop = currentLoop.next;
                    // ReSharper disable once RedundantAssignment
                    currentLoop = null;
                }
                nextPolygon = currentPolygon.next;
                if (skip)
                {
                    // do not free the input polygon
                    skip = false;
                } else {
                    // ReSharper disable once RedundantAssignment
                    currentPolygon = null;
                }
            }
        }

        /// <summary>
        /// Count the number of polygons in a linked list
        /// </summary>
        /// <param name="polygon">Starting polygon</param>
        /// <returns>Count</returns>
        /// <!-- Based off 3.1.1 -->
        public static int countLinkedPolygons(ref LinkedGeoPolygon polygon)
        {
            var polyIndex = polygon;
            int count = 0;
            while (polyIndex != null)
            {
                count++;
                polyIndex = polyIndex.next;
            }
            return count;
        }

        /// <summary>
        /// Count the number of linked loops in a polygon
        /// </summary>
        /// <param name="polygon">Polygon to count loops for</param> 
        /// <returns>Count</returns>
        /// <!-- Based off 3.1.1 -->
        public static int countLinkedLoops(ref LinkedGeoPolygon polygon)
        {
            LinkedGeoLoop loop = polygon.first;
            int count = 0;
            while (loop != null)
            {
                count++;
                loop = loop.next;
            }
            return count;
        }

        /// <summary>
        /// Count the number of coordinates in a loop
        /// </summary>
        /// <param name="loop"> Loop to count coordinates for</param>
        /// <returns>Count</returns>
        /// <!-- Based off 3.1.1 -->
        public static int countLinkedCoords(ref LinkedGeoLoop loop)
        {
            LinkedGeoCoord coord = loop.first;
            int count = 0;
            while (coord != null)
            {
                count++;
                coord = coord.next;
            }
            return count;
        }

        /// <summary>
        /// Count the number of polygons containing a given loop.
        /// </summary>
        /// <param name="loop">Loop to count containers for</param>
        /// <param name="polygons">Polygons to test</param>
        /// <param name="bboxes">Bounding boxes for polygons, used in point-in-poly check</param>
        /// <param name="polygonCount">Number of polygons in the test array</param>
        /// <returns>Number of polygons containing the loop
        /// <!-- Based off 3.1.1 -->
        public static int countContainers(
            LinkedGeoLoop loop, List<LinkedGeoPolygon> polygons,
            List<BBox> bboxes, int polygonCount)
        {
            int containerCount = 0;
            for (int i = 0; i < polygonCount; i++)
            {
                var bb = bboxes[i];
                if (loop != polygons[i].first &&
                    pointInsideLinkedGeoLoop(ref polygons[i].first, ref bb, ref loop.first.vertex))
                {
                    containerCount++;
                }
            }
            return containerCount;
        }

        /// <summary>
        /// Given a list of nested containers, find the one most deeply nested.
        /// </summary>
        /// <param name="polygons">Polygon containers to check</param>
        /// <param name="bboxes">Bounding boxes for polygons, used in point-in-poly check</param>
        /// <param name="polygonCount">Number of polygons in the list</param>
        /// <returns>Deepest container, or null if list is empty</returns>
        /// <!-- Based off 3.1.1 -->
        public static LinkedGeoPolygon findDeepestContainer(
        ref List<LinkedGeoPolygon> polygons, ref List<BBox> bboxes,
        int polygonCount) {
            // Set the initial return value to the first candidate
            LinkedGeoPolygon parent = polygonCount > 0 ? polygons[0] : null;

            // If we have multiple polygons, they must be nested inside each other.
            // Find the innermost polygon by taking the one with the most containers
            // in the list.
            if (polygonCount <= 1)
            {
                return parent;
            }
            int max = -1;
            for (int i = 0; i < polygonCount; i++) 
            {
                int count = countContainers(polygons[i].first, polygons, bboxes, polygonCount);
                if (count <= max)
                {
                    continue;
                }
                parent = polygons[i];
                max = count;
            }
            return parent;
        }

        /// <summary>
        /// Find the polygon to which a given hole should be allocated. Note that this
        /// function will return null if no parent is found.
        /// </summary>
        /// <param name="loop">Inner loop describing a hole</param>
        /// <param name="polygon">Head of a linked list of polygons to check</param>
        /// <param name="bboxes">Bounding boxes for polygons, used in point-in-poly check</param>
        /// <param name="polygonCount">Number of polygons to check</param>
        /// <returns>Pointer to parent polygon, or null if not found</returns>
        /// <!-- Based off 3.1.1 -->
        public static LinkedGeoPolygon findPolygonForHole(
            ref LinkedGeoLoop loop,
            ref LinkedGeoPolygon polygon,
            ref List<BBox> bboxes,
            int polygonCount)
        {
            // Early exit with no polygons
            if (polygonCount == 0) {
                return null;
            }
            // Initialize arrays for candidate loops and their bounding boxes
            List<LinkedGeoPolygon> candidates = new List<LinkedGeoPolygon>(polygonCount);
            List<BBox> candidateBBoxes = new List<BBox>(polygonCount);
            for (var k = 0; k < polygonCount; k++)
            {
                candidates.Add(new LinkedGeoPolygon());
                candidateBBoxes.Add(new BBox());
            }

            // Find all polygons that contain the loop
            int candidateCount = 0;
            int index = 0;
            var polygonReference = polygon;
            while (polygonReference != null) {
                // We are guaranteed not to overlap, so just test the first point
                var bb = bboxes[index];
                if (
                    pointInsideLinkedGeoLoop(
                        ref polygonReference.first, ref bb, ref loop.first.vertex
                    )
                )
                {
                    candidates[candidateCount] = polygonReference;
                    candidateBBoxes[candidateCount] = bboxes[index];
                    candidateCount++;
                }
                polygonReference = polygonReference.next;
                index++;
            }

            // The most deeply nested container is the immediate parent
            LinkedGeoPolygon parent =
                findDeepestContainer(ref candidates, ref candidateBBoxes, candidateCount);

            // Free allocated memory
            candidates = null;
            candidateBBoxes = null;
            return parent;
        }

        /// <summary>
        /// Normalize a LinkedGeoPolygon in-place into a structure following GeoJSON
        /// MultiPolygon rules: Each polygon must have exactly one outer loop, which
        /// must be first in the list, followed by any holes. Holes in this algorithm
        /// are identified by winding order (holes are clockwise), which is guaranteed
        /// by the h3SetToVertexGraph algorithm.
        /// 
        /// Input to this function is assumed to be a single polygon including all
        /// loops to normalize. It's assumed that a valid arrangement is possible.
        /// </summary>
        /// <param name="root">Root polygon including all loops</param>
         /// <returns>0 on success, or an error code > 0 for invalid input</returns>
        /// <!-- Based off 3.1.1 -->
        public static int normalizeMultiPolygon(ref LinkedGeoPolygon root)
        {
            // We assume that the input is a single polygon with loops;
            // if it has multiple polygons, don't touch it
            if (root.next != null)
            {
                return NORMALIZATION_ERR_MULTIPLE_POLYGONS;
            }

            // Count loops, exiting early if there's only one
            int loopCount = countLinkedLoops(ref root);
            if (loopCount <= 1)
            {
                return NORMALIZATION_SUCCESS;
            }

            int resultCode = NORMALIZATION_SUCCESS;
            LinkedGeoPolygon polygon = null;
            LinkedGeoLoop next = new LinkedGeoLoop();
            int innerCount = 0;
            int outerCount = 0;

            // Create an array to hold all of the inner loops. Note that
            // this array will never be full, as there will always be fewer
            // inner loops than outer loops.
            List<LinkedGeoLoop> innerLoops = new List<LinkedGeoLoop>(loopCount);
            for (var k = 0; k < loopCount; k++)
            {
                innerLoops.Add(new LinkedGeoLoop());
            }
            // Create an array to hold the bounding boxes for the outer loops
            List<BBox> bboxes = new List<BBox>(loopCount);
            for (var k = 0; k < loopCount; k++)
            {
                bboxes.Add(new BBox());
            }

            // Get the first loop and unlink it from root
            LinkedGeoLoop loop = root.first;
            root = new LinkedGeoPolygon();

            // Iterate over all loops, moving inner loops into an array and
            // assigning outer loops to new polygons
            while (loop != null)
            {
                if (isClockwiseLinkedGeoLoop(loop))
                {
                    innerLoops[innerCount] = loop;
                    innerCount++;
                }
                else
                {
                    polygon = polygon == null ? root : addNewLinkedPolygon(ref polygon);
                    addLinkedLoop(ref polygon, ref loop);
                    var bb = bboxes[outerCount];
                    bboxFromLinkedGeoLoop(ref loop, ref bb);
                    bboxes[outerCount] = bb;
                    outerCount++;
                }

                // get the next loop and unlink it from this one
                next = loop.next;
                loop.next = null;
                loop = next;
            }

            // Find polygon for each inner loop and assign the hole to it
            for (int i = 0; i < innerCount; i++)
            {
                var inner1 = innerLoops[i];
                polygon = findPolygonForHole(ref inner1, ref root, ref bboxes, outerCount);
                if (polygon != null)
                {
                    var inner2 = innerLoops[i];
                    addLinkedLoop(ref polygon, ref inner2);
                    innerLoops[i] = inner2;
                }
                else
                {
                    // If we can't find a polygon (possible with invalid input), then
                    // we need to release the memory for the hole, because the loop has
                    // been unlinked from the root and the caller will no longer have
                    // a way to destroy it with destroyLinkedPolygon.
                    var inner2 = innerLoops[i];
                    destroyLinkedGeoLoop(ref inner2);
                    innerLoops[i] = null;
                    resultCode = NORMALIZATION_ERR_UNASSIGNED_HOLES;
                }
            }
            return resultCode;
        }
    }
}
