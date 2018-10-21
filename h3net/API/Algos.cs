﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace h3net.API
{
    public class Algos
    {
        /*      _
         *    _/ \_      Directions used for traversing a        
         *   / \5/ \     hexagonal ring counterclockwise
         *   \0/ \4/     around {1, 0, 0}
         *   / \_/ \
         *   \1/ \3/
         *     \2/
         */

        public static readonly Direction[] DIRECTIONS =
        {
            Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
            Direction.K_AXES_DIGIT, Direction.IK_AXES_DIGIT,
            Direction.I_AXES_DIGIT, Direction.IJ_AXES_DIGIT
        };

        /**
         * New digit when traversing along class II grids.
         *
         * Current digit -> direction -> new digit.
         */
        public static readonly Direction[,] NEW_DIGIT_II =
        {
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT
            },
            {
                Direction.K_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                Direction.IK_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.CENTER_DIGIT
            },
            {
                Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.K_AXES_DIGIT, Direction.I_AXES_DIGIT,
                Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT
            },
            {
                Direction.JK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT
            },
            {
                Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.K_AXES_DIGIT
            },
            {
                Direction.IK_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT,
                Direction.JK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.I_AXES_DIGIT
            },
            {
                Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT, Direction.J_AXES_DIGIT,
                Direction.K_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.JK_AXES_DIGIT
            }
        };

        /**
         * New traversal direction when traversing along class II grids.
         *
         * Current digit -> direction -> new ap7 move (at coarser level).
         */
        public static readonly Direction[,] NEW_ADJUSTMENT_II =
        {
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT, Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.I_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.IJ_AXES_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.IK_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.IJ_AXES_DIGIT
            }
        };

        /**
         * New traversal direction when traversing along class III grids.
         *
         * Current digit -> direction -> new ap7 move (at coarser level).
         */
        public static readonly Direction[,] NEW_DIGIT_III =
        {
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT
            },
            {
                Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT,
                Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT
            },
            {
                Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT,
                Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT
            },
            {
                Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT
            },
            {
                Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT
            },
            {
                Direction.IK_AXES_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT,
                Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT
            },
            {
                Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.J_AXES_DIGIT,
                Direction.JK_AXES_DIGIT, Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT
            }
        };

        /**
         * New traversal direction when traversing along class III grids.
         *
         * Current digit -> direction -> new ap7 move (at coarser level).
         */
        public static readonly Direction[,] NEW_ADJUSTMENT_III =
        {
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.J_AXES_DIGIT, Direction.J_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.IJ_AXES_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.JK_AXES_DIGIT, Direction.J_AXES_DIGIT, Direction.JK_AXES_DIGIT,
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.I_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.I_AXES_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.K_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.CENTER_DIGIT,
                Direction.IK_AXES_DIGIT, Direction.IK_AXES_DIGIT, Direction.CENTER_DIGIT
            },
            {
                Direction.CENTER_DIGIT, Direction.CENTER_DIGIT, Direction.IJ_AXES_DIGIT, Direction.CENTER_DIGIT,
                Direction.I_AXES_DIGIT, Direction.CENTER_DIGIT, Direction.IJ_AXES_DIGIT
            }
        };


        /**
         * Maximum number of indices that result from the kRing algorithm with the given
         * k. Formula source and proof: https://oeis.org/A003215
         *
         * @param k k value, k >= 0.
         */
        public static int maxKringSize(int k)
        {
            int result = 1;
            for (int i = 0; i < k; i++) {
                result = result + 6 * (i + 1);
            }
            return result;
        }

        /**
         * k-rings produces indices within k distance of the origin index.
         *
         * k-ring 0 is defined as the origin index, k-ring 1 is defined as k-ring 0 and
         * all neighboring indices, and so on.
         *
         * Output is placed in the provided array in no particular order. Elements of
         * the output array may be left zero, as can happen when crossing a pentagon.
         *
         * @param origin Origin location.
         * @param k k >= 0
         * @param out Zero-filled array which must be of size maxKringSize(k).
         */
        public static void kRing(H3Index origin, int k, ref List<H3Index> out_hex)
        {
            int maxIdx = maxKringSize(k);
            var distances = new List<int>(maxIdx);
            Algos.kRingDistances(origin, k, ref out_hex, ref distances);
        }

        /**
         * k-rings produces indices within k distance of the origin index.
         *
         * k-ring 0 is defined as the origin index, k-ring 1 is defined as k-ring 0 and
         * all neighboring indices, and so on.
         *
         * Output is placed in the provided array in no particular order. Elements of
         * the output array may be left zero, as can happen when crossing a pentagon.
         *
         * @param origin Origin location.
         * @param k k >= 0
         * @param out Zero-filled array which must be of size maxKringSize(k).
         * @param distances Zero-filled array which must be of size maxKringSize(k).
         */
        public static void kRingDistances(H3Index origin, int k, ref List<H3Index> out_hex, ref List<int> distances)
        {
            int maxIdx = maxKringSize(k);
            // Optimistically try the faster hexRange algorithm first
            bool failed = hexRangeDistances(origin, k, ref out_hex, ref distances) != 0;
            if (failed)
            {
                // Fast algo failed, fall back to slower, correct algo
                // and also wipe out array because contents untrustworthy
                distances.Clear();
                distances = new int[out_hex.Count].ToList();
                _kRingInternal(origin, k, ref out_hex, ref distances, maxIdx, 0);
            }
        }

        /**
         * Internal helper function called recursively for kRingDistances.
         *
         * Adds the origin index to the output set (treating it as a hash set)
         * and recurses to its neighbors, if needed.
         *
         * @param origin
         * @param k Maximum distance to move from the origin.
         * @param out Array treated as a hash set, elements being either H3Index or 0.
         * @param distances Scratch area, with elements paralleling the out array.
         * Elements indicate ijk distance from the origin index to the output index.
         * @param maxIdx Size of out and scratch arrays (must be maxKringSize(k))
         * @param curK Current distance from the origin.
         */
        public static void _kRingInternal(H3Index origin, int k, ref List<H3Index> out_hex, ref List<int> distances,
            int maxIdx, int curK)
        {
            if (origin == 0) return;

            // Put origin in the output array. out is used as a hash set.
            ulong origin1 = (ulong) origin;
            int off = (int)( origin1 % (ulong)maxIdx);
            while (out_hex[off] != 0 && out_hex[off] != origin)
            {
                off = (off + 1) % maxIdx;
            }

            // We either got a free slot in the hash set or hit a duplicate
            // We might need to process the duplicate anyways because we got
            // here on a longer path before.
            if (out_hex[off] == origin && distances[off] <= curK)
            {
                return;
            }

            out_hex[off] = origin;
            distances[off] = curK;

            // Base case: reached an index k away from the origin.
            if (curK >= k)
            {
                return;
            }

            // Recurse to all neighbors in no particular order.
            for (int i = 0; i < 6; i++)
            {
                int rotations = 0;
                _kRingInternal(
                    h3NeighborRotations(origin, DIRECTIONS[i], ref rotations),
                    k, ref out_hex, ref distances, maxIdx, curK + 1);
            }

        }

        /**
         * Returns the hexagon index neighboring the origin, in the direction dir.
         *
         * Implementation note: The only reachable case where this returns 0 is if the
         * origin is a pentagon and the translation is in the k direction. Thus,
         * 0 can only be returned if origin is a pentagon.
         *
         * @param origin Origin index
         * @param dir Direction to move in
         * @param rotations Number of ccw rotations to perform to reorient the
         *                  translation vector. Will be modified to the new number of
         *                  rotations to perform (such as when crossing a face edge.)
         * @return H3Index of the specified neighbor or 0 if deleted k-subsequence
         *         distortion is encountered.
         */
        public static ulong h3NeighborRotations(H3Index origin, Direction dir, ref int rotations)
        {
            H3Index out_hex = origin;

            for (int i = 0; i < rotations; i++)
            {
                dir = CoordIJK._rotate60ccw(dir);
            }

            int newRotations = 0;
            int oldBaseCell = H3Index.H3_GET_BASE_CELL(out_hex);
            Direction oldLeadingDigit = H3Index._h3LeadingNonZeroDigit(out_hex);

            // Adjust the indexing digits and, if needed, the base cell.
            int r = H3Index.H3_GET_RESOLUTION(out_hex) - 1;
            while (true)
            {
                if (r == -1)
                {
                    H3Index.H3_SET_BASE_CELL(ref out_hex, BaseCells.baseCellNeighbors[oldBaseCell, (int) dir]);
                    newRotations = BaseCells.baseCellNeighbor60CCWRots[oldBaseCell, (int) dir];

                    if (H3Index.H3_GET_BASE_CELL(out_hex) == BaseCells.INVALID_BASE_CELL)
                    {
                        // Adjust for the deleted k vertex at the base cell level.
                        // This edge actually borders a different neighbor.
                        H3Index.H3_SET_BASE_CELL(ref out_hex,
                            BaseCells.baseCellNeighbors[oldBaseCell, (int) Direction.IK_AXES_DIGIT]);
                        newRotations =
                            BaseCells.baseCellNeighbor60CCWRots[oldBaseCell, (int) Direction.IK_AXES_DIGIT];

                        // perform the adjustment for the k-subsequence we're skipping
                        // over.
                        out_hex = H3Index._h3Rotate60ccw(ref out_hex);
                        rotations++;
                    }

                    break;
                }
                else
                {
                    Direction oldDigit = H3Index.H3_GET_INDEX_DIGIT(out_hex, r + 1);
                    Direction nextDir;
                    if (H3Index.isResClassIII(r + 1))
                    {
                        H3Index.H3_SET_INDEX_DIGIT(ref out_hex, r + 1, (ulong) NEW_DIGIT_II[(int) oldDigit, (int) dir]);
                        nextDir = NEW_ADJUSTMENT_II[(int) oldDigit, (int) dir];
                    }
                    else
                    {
                        H3Index.H3_SET_INDEX_DIGIT(ref out_hex, r + 1,
                            (ulong) NEW_DIGIT_III[(int) oldDigit, (int) dir]);
                        nextDir = NEW_ADJUSTMENT_III[(int) oldDigit, (int) dir];
                    }

                    if (nextDir != Direction.CENTER_DIGIT)
                    {
                        dir = nextDir;
                        r--;
                    }
                    else
                    {
                        // No more adjustment to perform
                        break;
                    }
                }
            }

            int newBaseCell = H3Index.H3_GET_BASE_CELL(out_hex);
            if (BaseCells._isBaseCellPentagon(newBaseCell))
            {
                int alreadyAdjustedKSubsequence = 0;

                // force rotation out of missing k-axes sub-sequence
                if (H3Index._h3LeadingNonZeroDigit(out_hex) == Direction.K_AXES_DIGIT)
                {
                    if (oldBaseCell != newBaseCell)
                    {
                        // in this case, we traversed into the deleted
                        // k subsequence of a pentagon base cell.
                        // We need to rotate out of that case depending
                        // on how we got here.
                        // check for a cw/ccw offset face; default is ccw

                        if (BaseCells._baseCellIsCwOffset(
                            newBaseCell, BaseCells.baseCellData[oldBaseCell].homeFijk.face))
                        {
                            out_hex = H3Index._h3Rotate60cw(ref out_hex);
                        }
                        else
                        {
                            // See cwOffsetPent in testKRing.c for why this is
                            // unreachable.
                            out_hex = H3Index._h3Rotate60ccw(ref out_hex); // LCOV_EXCL_LINE
                        }

                        alreadyAdjustedKSubsequence = 1;
                    }
                    else
                    {
                        // In this case, we traversed into the deleted
                        // k subsequence from within the same pentagon
                        // base cell.
                        if (oldLeadingDigit == Direction.CENTER_DIGIT)
                        {
                            // Undefined: the k direction is deleted from here
                            return H3Index.H3_INVALID_INDEX;
                        }
                        else if (oldLeadingDigit == Direction.JK_AXES_DIGIT)
                        {
                            // Rotate out of the deleted k subsequence
                            // We also need an additional change to the direction we're
                            // moving in
                            out_hex = H3Index._h3Rotate60ccw(ref out_hex);
                            rotations++;
                        }
                        else if (oldLeadingDigit == Direction.IK_AXES_DIGIT)
                        {
                            // Rotate out of the deleted k subsequence
                            // We also need an additional change to the direction we're
                            // moving in
                            out_hex = H3Index._h3Rotate60cw(ref out_hex);
                            rotations += 5;
                        }
                        else
                        {
                            // Should never occur
                            return H3Index.H3_INVALID_INDEX; // LCOV_EXCL_LINE
                        }
                    }
                }

                for (int i = 0; i < newRotations; i++)
                {
                    out_hex = H3Index._h3RotatePent60ccw(ref out_hex);
                }

                // Account for differing orientation of the base cells (this edge
                // might not follow properties of some other edges.)
                if (oldBaseCell != newBaseCell)
                {
                    if (newBaseCell == 4 || newBaseCell == 117)
                    {
                        // 'polar' base cells behave differently because they have all
                        // i neighbors.
                        if (oldBaseCell != 118 && oldBaseCell != 8 &&
                            H3Index._h3LeadingNonZeroDigit(out_hex) != Direction.JK_AXES_DIGIT)
                        {
                            rotations++;
                        }
                    }
                    else if (H3Index._h3LeadingNonZeroDigit(out_hex) == Direction.IK_AXES_DIGIT &&
                             alreadyAdjustedKSubsequence == 0)
                    {
                        // account for distortion introduced to the 5 neighbor by the
                        // deleted k subsequence.
                        rotations++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < newRotations; i++)
                {
                    out_hex = H3Index._h3Rotate60ccw(ref out_hex);
                }
            }

            rotations = (rotations + newRotations) % 6;

            return out_hex;
        }

        /**
         * hexRange produces indexes within k distance of the origin index.
         * Output behavior is undefined when one of the indexes returned by this
         * function is a pentagon or is in the pentagon distortion area.
         *
         * k-ring 0 is defined as the origin index, k-ring 1 is defined as k-ring 0 and
         * all neighboring indexes, and so on.
         *
         * Output is placed in the provided array in order of increasing distance from
         * the origin.
         *
         * @param origin Origin location.
         * @param k k >= 0
         * @param out Array which must be of size maxKringSize(k).
         * @return 0 if no pentagon or pentagonal distortion area was encountered.
         */
        public static int hexRange(H3Index origin, int k, ref List<H3Index> out_hex)
        {
            //  Can't pass in a 0 as a null pointer, so we'll
            //  check against an empty list.
            var fake_distance = new List<int>();
            return hexRangeDistances(origin, k, ref out_hex, ref fake_distance);
        }

        /**
         * hexRange produces indexes within k distance of the origin index.
         * Output behavior is undefined when one of the indexes returned by this
         * function is a pentagon or is in the pentagon distortion area.
         *
         * k-ring 0 is defined as the origin index, k-ring 1 is defined as k-ring 0 and
         * all neighboring indexes, and so on.
         *
         * Output is placed in the provided array in order of increasing distance from
         * the origin. The distances in hexagons is placed in the distances array at
         * the same offset.
         *
         * @param origin Origin location.
         * @param k k >= 0
         * @param out Array which must be of size maxKringSize(k).
         * @param distances Null or array which must be of size maxKringSize(k).
         * @return 0 if no pentagon or pentagonal distortion area was encountered.
         */
        public static int hexRangeDistances(H3Index origin, int k, ref List<H3Index> out_size, ref List<int> distances)
        {
            // Return codes:
            // 1 Pentagon was encountered
            // 2 Pentagon distortion (deleted k subsequence) was encountered
            // Pentagon being encountered is not itself a problem; really the deleted
            // k-subsequence is the problem, but for compatibility reasons we fail on
            // the pentagon.

            for (int m = 0; m < out_size.Count; m++)
            {
                //out_size.Add(0);
                distances.Add(0);
            }
            // k must be >= 0, so origin is always needed
            int idx = 0;
            out_size[idx] = origin;
            distances[0] = 0;
            idx++;

            if (H3Index.h3IsPentagon(origin) > 0)
            {
                // Pentagon was encountered; bail out as user doesn't want this.
                return Constants.HEX_RANGE_PENTAGON;
            }

            // 0 < ring <= k, current ring
            int ring = 1;
            // 0 <= direction < 6, current side of the ring
            int direction = 0;
            // 0 <= i < ring, current position on the side of the ring
            int i = 0;
            // Number of 60 degree ccw rotations to perform on the direction (based on
            // which faces have been crossed.)
            int rotations = 0;

            while (ring <= k)
            {
                if (direction == 0 && i == 0)
                {
                    // Not putting in the output set as it will be done later, at
                    // the end of this ring.
                    origin = h3NeighborRotations(origin, Constants.NEXT_RING_DIRECTION, ref rotations);
                    if (origin == 0)
                    {
                        // Should not be possible because `origin` would have to be a
                        // pentagon
                        return Constants.HEX_RANGE_K_SUBSEQUENCE; // LCOV_EXCL_LINE
                    }

                    if (H3Index.h3IsPentagon(origin) > 0)
                    {
                        // Pentagon was encountered; bail out as user doesn't want this.
                        return Constants.HEX_RANGE_PENTAGON;
                    }
                }

                origin = h3NeighborRotations(origin, DIRECTIONS[direction], ref rotations);
                if (origin == 0)
                {
                    // Should not be possible because `origin` would have to be a
                    // pentagon
                    return Constants.HEX_RANGE_K_SUBSEQUENCE; // LCOV_EXCL_LINE
                }

                out_size[idx] = origin;
                if (distances.Count > 0)
                {
                    distances[idx] = ring;
                }

                idx++;

                i++;
                // Check if end of this side of the k-ring
                if (i == ring)
                {
                    i = 0;
                    direction++;
                    // Check if end of this ring.
                    if (direction == 6)
                    {
                        direction = 0;
                        ring++;
                    }
                }

                if (H3Index.h3IsPentagon(origin) > 0)
                {
                    // Pentagon was encountered; bail out as user doesn't want this.
                    return Constants.HEX_RANGE_PENTAGON;
                }
            }

            return Constants.HEX_RANGE_SUCCESS;
        }

        /**
         * hexRanges takes an array of input hex IDs and a max k-ring and returns an
         * array of hexagon IDs sorted first by the original hex IDs and then by the
         * k-ring (0 to max), with no guaranteed sorting within each k-ring group.
         *
         * @param h3Set A pointer to an array of H3Indexes
         * @param length The total number of H3Indexes in h3Set
         * @param k The number of rings to generate
         * @param out A pointer to the output memory to dump the new set of H3Indexes to
         *            The memory block should be equal to maxKringSize(k) * length
         * @return 0 if no pentagon is encountered. Cannot trust output otherwise
         */
        public static int hexRanges(ref List<H3Index> h3Set, int length, int k, List<H3Index> out_index)
        {
            //List<H3Index> segment = new List<H3Index>();
            int segmentSize = maxKringSize(k);
            for (int i = 0; i < length; i++)
            {
                //  Take a slice.
                List<H3Index> segment = out_index.GetRange(i * segmentSize, segmentSize);
                // Determine the appropriate segment of the output array to operate on
                var success = hexRange(h3Set[i], k, ref segment);
                //  put the slice back.
                for (int m = 0; m < segmentSize; m++)
                {
                    out_index[i * segmentSize + m] = segment[m];
                }

                if (success != 0)
                {
                    return success;
                }
            }

            return 0;
        }

        /**
         * Returns the hollow hexagonal ring centered at origin with sides of length k.
         *
         * @param origin Origin location.
         * @param k k >= 0
         * @param out Array which must be of size 6 * k (or 1 if k == 0)
         * @return 0 if no pentagonal distortion was encountered.
         */
        public static int hexRing(H3Index origin, int k, ref List<H3Index> out_hex)
        {
            // Short-circuit on 'identity' ring
            if (k == 0)
            {
                out_hex[0] = origin;
                return 0;
            }

            int idx = 0;
            // Number of 60 degree ccw rotations to perform on the direction (based on
            // which faces have been crossed.)
            int rotations = 0;
            // Scratch structure for checking for pentagons
            if (H3Index.h3IsPentagon(origin) > 0)
            {
                // Pentagon was encountered; bail out as user doesn't want this.
                return Constants.HEX_RANGE_PENTAGON;
            }

            for (int ring = 0; ring < k; ring++)
            {
                origin = h3NeighborRotations(origin, Constants.NEXT_RING_DIRECTION, ref rotations);
                if (origin == 0)
                {
                    // Should not be possible because `origin` would have to be a
                    // pentagon
                    return Constants.HEX_RANGE_K_SUBSEQUENCE; // LCOV_EXCL_LINE
                }

                if (H3Index.h3IsPentagon(origin) > 0)
                {
                    return Constants.HEX_RANGE_PENTAGON;
                }
            }

            H3Index lastIndex = origin;

            out_hex[idx] = origin;
            idx++;

            for (int direction = 0; direction < 6; direction++)
            {
                for (int pos = 0; pos < k; pos++)
                {
                    origin =
                        h3NeighborRotations(origin, DIRECTIONS[direction], ref rotations);
                    if (origin == 0)
                    {
                        // Should not be possible because `origin` would have to be a
                        // pentagon
                        return Constants.HEX_RANGE_K_SUBSEQUENCE; // LCOV_EXCL_LINE
                    }

                    // Skip the very last index, it was already added. We do
                    // however need to traverse to it because of the pentagonal
                    // distortion check, below.
                    if (pos != k - 1 || direction != 5)
                    {
                        out_hex[idx] = origin;
                        idx++;

                        if (H3Index.h3IsPentagon(origin) > 0)
                        {
                            return Constants.HEX_RANGE_PENTAGON;
                        }
                    }
                }
            }

            // Check that this matches the expected lastIndex, if it doesn't,
            // it indicates pentagonal distortion occurred and we should report
            // failure.
            if (lastIndex != origin)
            {
                return Constants.HEX_RANGE_PENTAGON;
            }
            else
            {
                return Constants.HEX_RANGE_SUCCESS;
            }
        }

        /**
         * maxPolyfillSize returns the number of hexagons to allocate space for when
         * performing a polyfill on the given GeoJSON-like data structure.
         *
         * Currently a laughably padded response, being a k-ring that wholly contains
         * a bounding box of the GeoJSON, but still less wasted memory than initializing
         * a Python application? ;)
         *
         * @param geoPolygon A GeoJSON-like data structure indicating the poly to fill
         * @param res Hexagon resolution (0-15)
         * @return number of hexagons to allocate for
         */
        public static int maxPolyfillSize(ref GeoPolygon geoPolygon, int res)
        {
            // Get the bounding box for the GeoJSON-like struct
            BBox bbox = new BBox();
            Polygon.bboxFromGeofence(ref geoPolygon.Geofence, ref bbox);
            int minK = BBox.bboxHexRadius(bbox, res);

            // The total number of hexagons to allocate can now be determined by
            // the k-ring hex allocation helper function.
            return maxKringSize(minK);
        }

        /**
         * polyfill takes a given GeoJSON-like data structure and preallocated,
         * zeroed memory, and fills it with the hexagons that are contained by
         * the GeoJSON-like data structure.
         *
         * The current implementation is very primitive and slow, but correct,
         * performing a point-in-poly operation on every hexagon in a k-ring defined
         * around the given Geofence.
         *
         * @param geoPolygon The Geofence and holes defining the relevant area
         * @param res The Hexagon resolution (0-15)
         * @param out The slab of zeroed memory to write to. Assumed to be big enough.
         */
        public static void polyfill(GeoPolygon geoPolygon, int res, List<H3Index> out_hex) {
            // One of the goals of the polyfill algorithm is that two adjacent polygons
            // with zero overlap have zero overlapping hexagons. That the hexagons are
            // uniquely assigned. There are a few approaches to take here, such as
            // deciding based on which polygon has the greatest overlapping area of the
            // hexagon, or the most number of contained points on the hexagon (using the
            // center point as a tiebreaker).
            //
            // But if the polygons are convex, both of these more complex algorithms can
            // be reduced down to checking whether or not the center of the hexagon is
            // contained in the polygon, and so this is the approach that this polyfill
            // algorithm will follow, as it's simpler, faster, and the error for concave
            // polygons is still minimal (only affecting concave shapes on the order of
            // magnitude of the hexagon size or smaller, not impacting larger concave
            // shapes)
            //
            // This first part is identical to the maxPolyfillSize above.

            // Get the bounding boxes for the polygon and any holes
            int cnt = geoPolygon.numHoles + 1;
            List<BBox> bboxes = new List<BBox>();
            for (int i = 0; i < cnt; i++)
            {
                bboxes.Add(new BBox());
            }

            Polygon.bboxesFromGeoPolygon(geoPolygon, ref bboxes);
            
            int minK = BBox.bboxHexRadius(bboxes[0], res);
            int numHexagons = maxKringSize(minK);

            // Get the center hex
            GeoCoord center = new GeoCoord();
            BBox.bboxCenter(bboxes[0], ref center);
            H3Index centerH3 = H3Index.geoToH3(ref center, res);

            // From here on it works differently, first we get all potential
            // hexagons inserted into the available memory
            kRing(centerH3, minK,ref  out_hex);

            // Next we iterate through each hexagon, and test its center point to see if
            // it's contained in the GeoJSON-like struct
            for (int i = 0; i < numHexagons; i++) {
                // Skip records that are already zeroed
                if (out_hex[i] == 0)
                {
                    continue;
                }
                // Check if hexagon is inside of polygon
                GeoCoord hexCenter = new GeoCoord();
                H3Index.h3ToGeo(out_hex[i], ref hexCenter);
                hexCenter.lat = GeoCoord.constrainLat(hexCenter.lat);
                hexCenter.lon = GeoCoord.constrainLng(hexCenter.lon);
                // And remove from list if not
                if (!Polygon.pointInsidePolygon(geoPolygon, bboxes, hexCenter))
                {
                    out_hex[i] = H3Index.H3_INVALID_INDEX;
                }
            }
        }

        /**
         * Internal: Create a vertex graph from a set of hexagons. It is the
         * responsibility of the caller to call destroyVertexGraph on the populated
         * graph, otherwise the memory in the graph nodes will not be freed.
         * @private
         * @param h3Set    Set of hexagons
         * @param numHexes Number of hexagons in the set
         * @param graph    Output graph
         */
        public static void h3SetToVertexGraph(ref List<H3Index> h3Set, int numHexes,
            ref VertexGraph graph)
        {
            GeoBoundary vertices = new GeoBoundary();
            GeoCoord fromVtx = new GeoCoord();
            GeoCoord toVtx = new GeoCoord();
            VertexGraph.VertexNode edge;
            if (numHexes < 1)
            {
                // We still need to init the graph, or calls to destroyVertexGraph will
                // fail
                graph = new VertexGraph(0, 0);
                return;
            }

            int res = H3Index.H3_GET_RESOLUTION(h3Set[0]);
            const int minBuckets = 6;
            // TODO: Better way to calculate/guess?
            int numBuckets = numHexes > minBuckets ? numHexes : minBuckets;
            graph = new VertexGraph(numBuckets, res);

            // Iterate through every hexagon
            for (int i = 0; i < numHexes; i++)
            {

                H3Index.h3ToGeoBoundary(h3Set[i], ref vertices);
                // iterate through every edge
                for (int j = 0; j < vertices.numVerts; j++)
                {
                    fromVtx = new GeoCoord(vertices.verts[j].lat, vertices.verts[j].lon);
                    //fromVtx = vertices.verts[j];
                    int idx = (j + 1) % vertices.numVerts;
                    toVtx = new GeoCoord(vertices.verts[idx].lat, vertices.verts[idx].lon);
                    //toVtx = vertices.verts[(j + 1) % vertices.numVerts];
                    // If we've seen this edge already, it will be reversed
                    edge = VertexGraph.findNodeForEdge(ref graph, toVtx, fromVtx);
                    if (edge != null)
                    {
                        // If we've seen it, drop it. No edge is shared by more than 2
                        // hexagons, so we'll never see it again.
                        VertexGraph.removeVertexNode(ref graph, ref edge);
                    }
                    else
                    {
                        // Add a new node for this edge
                        VertexGraph.addVertexNode(ref graph, fromVtx, toVtx);
                    }
                }
            }
        }

        /**
         * Internal: Create a LinkedGeoPolygon from a vertex graph. It is the
         * responsibility of the caller to call destroyLinkedPolygon on the populated
         * linked geo structure, or the memory for that structure will not be freed.
         * @private
         * @param graph Input graph
         * @param out   Output polygon
         */
        public static void _vertexGraphToLinkedGeo(ref VertexGraph graph, ref LinkedGeo.LinkedGeoPolygon out_polygon)
        {
            out_polygon = new LinkedGeo.LinkedGeoPolygon();
            LinkedGeo.LinkedGeoLoop loop;
            VertexGraph.VertexNode edge;
            GeoCoord nextVtx;
            // Find the next unused entry point
            while ((edge = VertexGraph.firstVertexNode(ref graph)) != null)
            {
                loop = LinkedGeo.addNewLinkedLoop(ref out_polygon);
                // Walk the graph to get the outline
                do
                {
                    var addLinkedCoord = LinkedGeo.addLinkedCoord(ref loop, ref edge.from);
                    nextVtx = edge.to;
                    // Remove frees the node, so we can't use edge after this
                    VertexGraph.removeVertexNode(ref graph, ref edge);
                    edge = VertexGraph.findNodeForVertex(ref graph, ref nextVtx);
                } while (edge != null);
            }
        }

        /**
         * Create a LinkedGeoPolygon describing the outline(s) of a set of  hexagons.
         * Polygon outlines will follow GeoJSON MultiPolygon order: Each polygon will
         * have one outer loop, which is first in the list, followed by any holes.
         *
         * It is the responsibility of the caller to call destroyLinkedPolygon on the
         * populated linked geo structure, or the memory for that structure will
         * not be freed.
         *
         * It is expected that all hexagons in the set have the same resolution and
         * that the set contains no duplicates. Behavior is undefined if duplicates
         * or multiple resolutions are present, and the algorithm may produce
         * unexpected or invalid output.
         *
         * @param h3Set    Set of hexagons
         * @param numHexes Number of hexagons in set
         * @param out      Output polygon
         */
        public static void h3SetToLinkedGeo(ref List<H3Index> h3Set, int numHexes,
            ref LinkedGeo.LinkedGeoPolygon out_polygons)
        {
            VertexGraph graph = new VertexGraph(0, 0);
            Algos.h3SetToVertexGraph(ref h3Set, numHexes, ref graph);
            Algos._vertexGraphToLinkedGeo(ref graph, ref out_polygons);
            // TODO: The return value, possibly indicating an error, is discarded here -
            // we should use this when we update the API to return a value
            LinkedGeo.normalizeMultiPolygon(ref out_polygons);
            VertexGraph.destroyVertexGraph(ref graph);
        }
    }
}